using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using RecycleMeDomainClasses;
using RecycleMeDataAccessLayer;
using System.Web;
using System.IO;

namespace RecycleMeOdataWebApi.Controllers
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using RecycleMeDomainClasses;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Item>("Item");
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ItemController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/Item
        [Queryable]
        public IQueryable<Item> GetItem()
        {
            return db.Items;
        }

        // GET odata/Item(5)
        [Queryable]
        public SingleResult<Item> GetItem([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Items.Where(item => item.Id == key));
        }

        // PUT odata/Item(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != item.Id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(item);
        }

        // POST odata/Item
        //{
        //"Name":"Shoes",
        //"ImagePath":"",
        //"Description":"",
        //"IsDeleted": false,
        //"ModifiedDate":"2014-04-07T16:11:53.333",
        //"OwnerId":"55fea526-02f2-4c1a-9ca4-ad47c8d717ae"
        //}
        public async Task<IHttpActionResult> Post(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);
            await db.SaveChangesAsync();

            return Created(item);
        }

        // PATCH odata/Item(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Item> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Item item = await db.Items.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }

            patch.Patch(item);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(item);
        }

        // DELETE odata/Item(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            Item item = await db.Items.FindAsync(key);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Item(5)/Owner
        [Queryable]
        public SingleResult<User> GetOwner([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.Items.Where(m => m.Id == key).Select(m => m.Owner));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(Guid key)
        {
            return db.Items.Count(e => e.Id == key) > 0;
        }



        [HttpPost]
        public int SetAlertFlag()
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }


            return 0;
        }


        private const string CONTAINER = "documents";

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
            blobContainer.CreateIfNotExists();

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

            #region [MultipartFormDataStreamProvider]
            //string root = HttpContext.Current.Server.MapPath("~/App_Data");
            //var provider = new MultipartFormDataStreamProvider(root);

            //try
            //{
            //    // Read the form data and return an async task. 
            //    await Request.Content.ReadAsMultipartAsync(provider);

            //    // This illustrates how to get the file names for uploaded files. 
            //    foreach (var fileData in provider.FileData)
            //    {
            //        var filename = fileData.LocalFileName;
            //        var blob = blobContainer.GetBlockBlobReference(filename);

            //        using (var filestream = File.OpenRead(fileData.LocalFileName))
            //        {
            //            blob.UploadFromStream(filestream);
            //        }
            //        File.Delete(fileData.LocalFileName);
            //    }

            //    return Request.CreateResponse(HttpStatusCode.OK);
            //}
            //catch (System.Exception e)
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            //}
            #endregion

        }

        public async Task<HttpResponseMessage> Get(string id)
        {
            var context = new StorageContext();

            // Get and create the container 
            var blobContainer = context.BlobClient.GetContainerReference(CONTAINER);
            blobContainer.CreateIfNotExists();

            var blob = blobContainer.GetBlockBlobReference(id);

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
