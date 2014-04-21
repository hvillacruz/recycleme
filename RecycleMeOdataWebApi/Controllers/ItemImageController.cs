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

namespace RecycleMeOdataWebApi.Controllers
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using RecycleMeDomainClasses;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<ItemImage>("ItemImage");
    builder.EntitySet<Item>("Items"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ItemImageController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/ItemImage
        [Queryable]
        public IQueryable<ItemImage> GetItemImage()
        {
            return db.ItemImage;
        }

        // GET odata/ItemImage(5)
        [Queryable]
        public SingleResult<ItemImage> GetItemImage([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemImage.Where(itemimage => itemimage.Id == key));
        }

        // PUT odata/ItemImage(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, ItemImage itemimage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != itemimage.Id)
            {
                return BadRequest();
            }

            db.Entry(itemimage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemImageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemimage);
        }

        // POST odata/ItemImage
        public async Task<IHttpActionResult> Post(ItemImage itemimage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ItemImage.Add(itemimage);
            await db.SaveChangesAsync();

            return Created(itemimage);
        }

        // PATCH odata/ItemImage(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<ItemImage> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ItemImage itemimage = await db.ItemImage.FindAsync(key);
            if (itemimage == null)
            {
                return NotFound();
            }

            patch.Patch(itemimage);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemImageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemimage);
        }

        // DELETE odata/ItemImage(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            ItemImage itemimage = await db.ItemImage.FindAsync(key);
            if (itemimage == null)
            {
                return NotFound();
            }

            db.ItemImage.Remove(itemimage);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/ItemImage(5)/Item
        [Queryable]
        public SingleResult<Item> GetItem([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemImage.Where(m => m.Id == key).Select(m => m.Item));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemImageExists(long key)
        {
            return db.ItemImage.Count(e => e.Id == key) > 0;
        }
    }
}
