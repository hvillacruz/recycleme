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

namespace RecycleMeOdataWebApi.Controllers
{
    public partial class ItemController : ODataController
    {

        // POST odata/Item
        //{
        //"Name":"Shoes",
        //"ImagePath":"",
        //"Description":"",
        //"IsDeleted": false,
        //"ModifiedDate":"2014-04-07T16:11:53.333",
        //"OwnerId":"55fea526-02f2-4c1a-9ca4-ad47c8d717ae"
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
                            blob.UploadFromStream(stream);

                        }
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
            #endregion


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