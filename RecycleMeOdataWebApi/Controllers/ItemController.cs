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
    builder.EntitySet<Item>("Item");
    builder.EntitySet<ItemCategory>("ItemCategory"); 
    builder.EntitySet<ItemComment>("ItemComment"); 
    builder.EntitySet<ItemImage>("ItemImage"); 
    builder.EntitySet<ItemFollowers>("ItemFollowers"); 
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public partial class ItemController : ODataController
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
        public SingleResult<Item> GetItem([FromODataUri] long key)
        {
            return SingleResult.Create(db.Items.Where(item => item.Id == key));
        }

        // PUT odata/Item(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, Item item)
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
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<Item> patch)
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
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
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

        // GET odata/Item(5)/Category
        [Queryable]
        public SingleResult<ItemCategory> GetCategory([FromODataUri] long key)
        {
            return SingleResult.Create(db.Items.Where(m => m.Id == key).Select(m => m.Category));
        }

        // GET odata/Item(5)/ItemCommented
        [Queryable]
        public IQueryable<ItemComment> GetItemCommented([FromODataUri] long key)
        {
            return db.Items.Where(m => m.Id == key).SelectMany(m => m.ItemCommented);
        }

        // GET odata/Item(5)/ItemImages
        [Queryable]
        public IQueryable<ItemImage> GetItemImages([FromODataUri] long key)
        {
            return db.Items.Where(m => m.Id == key).SelectMany(m => m.ItemImages);
        }

        // GET odata/Item(5)/ItemUserFollowers
        [Queryable]
        public IQueryable<ItemFollowers> GetItemUserFollowers([FromODataUri] long key)
        {
            return db.Items.Where(m => m.Id == key).SelectMany(m=> m.ItemUserFollowers);
        }

        // GET odata/Item(5)/Owner
        [Queryable]
        public SingleResult<User> GetOwner([FromODataUri] long key)
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

        private bool ItemExists(long key)
        {
            return db.Items.Count(e => e.Id == key) > 0;
        }
    }
}
