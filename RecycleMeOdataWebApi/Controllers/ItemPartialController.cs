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
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

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

                            //var newResizeStream = ImageResize(stream, System.Drawing.Imaging.ImageFormat.Jpeg, 1400);
                            //var newResizeStream = ImageResize(stream);
                            var newResizeStream = ImageResize(stream,550,500,false);
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


        public static System.IO.Stream ImageResize(System.IO.Stream inputStream, int Height, int Width, bool needToFill)
        {
            System.IO.Stream result = new System.IO.MemoryStream();
            System.Drawing.Image img = System.Drawing.Image.FromStream(inputStream);

            System.Drawing.Image thumbnail = FixedSize(img, Height, Width, needToFill);
            thumbnail.Save(result,ImageFormat.Jpeg);


            result.Seek(0, 0);

            return result;
        }


        public static System.IO.Stream ImageResize(System.IO.Stream inputStream)
        {

            EncoderParameters encodingParameters = new EncoderParameters(1);
            encodingParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L); // Set the JPG Quality percentage to 90%.
            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);   
            //ImageCodecInfo jpgEncoder = GetEncoderInfo("image/jpeg");

            // Incoming! This is the original image. This line can effectively be anything, but in this example it's coming from a stream.
            var image = System.Drawing.Image.FromStream(inputStream);

            // Creating two blank canvas. One that the original image is placed into, the other for the resized version.
            Bitmap originalImage = new Bitmap(image);
            Bitmap newImage = new Bitmap(originalImage, 1024, (image.Height * 768 / image.Width));  // Width of 300 & maintain aspect ratio (let it be as high as it needs to be).

            // We then do some funky voodoo with the newImage. Changing it to a graphic to allow us to set the HighQualityBilinear property and resize nicely.
            Graphics g = Graphics.FromImage(newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);

            var streamLarge = new System.IO.MemoryStream();
            newImage.Save(streamLarge, jpgEncoder, encodingParameters);

            //// This is the line that returns the picture to the relevant part of the model.
            //_event.Picture = streamLarge.ToArray();

            //// No need for all that drama for the thumbnail, the loss of quality isn't noticable.
            //var thumbnail = image.GetThumbnailImage(80, (image.Height * 80 / image.Width), null, new IntPtr(0));
            //var streamThumbnail = new System.IO.MemoryStream();

            //thumbnail.Save(streamThumbnail, jpgEncoder, encodingParameters);
            //_event.ThumbnailPicture = streamThumbnail.ToArray();

            // Good boy's tidy-up after themselves! :O
           // originalImage.Dispose();
           // newImage.Dispose();
           // thumbnail.Dispose();
           // streamLarge.Dispose();
           // streamThumbnail.Dispose();
            streamLarge.Seek(0, 0);
            return streamLarge;
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        public static Image FixedSize(Image imgPhoto, int Height, int Width, bool needToFill)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (!needToFill)
            {
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }
            }
            else
            {
                if (nPercentH > nPercentW)
                {
                    nPercent = nPercentH;
                    destX = (int)Math.Round((Width -
                        (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = (int)Math.Round((Height -
                        (sourceHeight * nPercent)) / 2);
                }
            }

            if (nPercent > 1)
                nPercent = 1;

            int destWidth = (int)Math.Round(sourceWidth * nPercent);
            int destHeight = (int)Math.Round(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(
                destWidth <= Width ? destWidth : Width,
                destHeight < Height ? destHeight : Height,
                              PixelFormat.Format32bppRgb);

            Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);
            grPhoto.InterpolationMode = InterpolationMode.Default;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;

            grPhoto.DrawImage(imgPhoto,
                new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
                new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                System.Drawing.GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
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