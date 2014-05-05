using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using RecycleMeDomainClasses;
using RecycleMeDataAccessLayer;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Net.Http.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using ExtensionMethods;
namespace ExtensionMethods
{
    public static class JSONHelper
    {
        public static string ToJSON(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static string ToJSON(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }
    }
}
namespace RecycleMeOdataWebApi.Controllers
{
    class ItemId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
    
    public partial class ItemController : ODataController
    {
        //{
        //"OwnerId":"bc943640-22f8-42d3-8646-c9e40938f34b",
        //"Name":"Item 1",
        //"ImagePath":"item.jpg",
        //"Description":"",
        //"ItemTag":"test",
        //"TradeTag":"trade",
        //"IsDeleted":false,
        //"ModifiedDate":"2014-04-20T18:10:33.96",
        //"ItemCategoryId":1
        //}

        private const string CONTAINER = "images";

        [HttpPost]
        public async Task<HttpResponseMessage> UploadFile()
        {
            var context = new StorageContext();

            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // Get and create the container 
            var blobContainer = context.BlobClient.GetContainerReference(CONTAINER);

            // Create the "images" container if it doesn't already exist.
            if (await blobContainer.CreateIfNotExistsAsync())
            {
                // Enable public access on the newly created "images" container
                await blobContainer.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess =
                            BlobContainerPublicAccessType.Blob
                    });


            }


            #region [MultipartMemoryStreamProvider]
            List<ItemId> id = new List<ItemId>();
            try
            {
                if (Request.Content.IsMimeMultipartContent())
                {
               
                    var streamProvider = new MultipartMemoryStreamProvider();
                    await Request.Content.ReadAsMultipartAsync(streamProvider).ContinueWith(t =>
                    {
                        foreach (var fileContent in streamProvider.Contents)
                        {

                            Stream stream = fileContent.ReadAsStreamAsync().Result;
                            var fileName = fileContent.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                            var blob = blobContainer.GetBlockBlobReference(fileName);

                            var newResizeStream = ImageResize(stream, System.Drawing.Imaging.ImageFormat.Jpeg, 1400);
                            blob.UploadFromStream(newResizeStream);

                            ItemImage image = new ItemImage
                            {
                                Name = fileName,
                                Path = blob.StorageUri.PrimaryUri.AbsoluteUri
                            };
                           
                            db.ItemImage.Add(image);
                            db.SaveChanges();

                            id.Add(new ItemId { 
                                Id = image.Id,
                                Name = image.Name,
                                Path = image.Path
                            });
                        }
                    });
                }
              
                  
                var response = this.Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(id.ToJSON(), Encoding.UTF8, "application/json");
           
                return response;
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            #endregion


        }



        public static System.IO.Stream ImageResize(System.IO.Stream inputStream, System.Drawing.Imaging.ImageFormat contentType, Int32 maximumDimension)
        {
            System.IO.Stream result = new System.IO.MemoryStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream);
            Int32 thumbnailWidth = (img.Width > img.Height) ? maximumDimension : img.Width * maximumDimension / img.Height;
            Int32 thumbnailHeight = (img.Width > img.Height) ? img.Height * maximumDimension / img.Width : maximumDimension;
            System.Drawing.Image thumbnail = img.GetThumbnailImage(thumbnailWidth, thumbnailHeight, null, IntPtr.Zero);
            thumbnail.Save(result, contentType);

            result.Seek(0, 0);

            return result;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> DownloadFile(ODataActionParameters parameters)
        {
            var context = new StorageContext();

            // Get and create the container 
            var blobContainer = context.BlobClient.GetContainerReference(CONTAINER);
            blobContainer.CreateIfNotExists();

            var blob = blobContainer.GetBlockBlobReference((string)parameters["name"]);

            var blobExists = await blob.ExistsAsync();
            if (!blobExists)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found");
            }

            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            Stream blobStream = await blob.OpenReadAsync();

            message.Content = new StreamContent(blobStream);
            message.Content.Headers.ContentLength = blob.Properties.Length;
            message.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(blob.Properties.ContentType);
            message.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = blob.Name,
                Size = blob.Properties.Length
            };

            return message;
        }


    }
}