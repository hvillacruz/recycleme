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
    builder.EntitySet<ItemCategory>("ItemCategory");
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ItemCategoryController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/ItemCategory
        [Queryable]
        public IQueryable<ItemCategory> GetItemCategory()
        {
            return db.ItemCategory;
        }

        // GET odata/ItemCategory(5)
        [Queryable]
        public SingleResult<ItemCategory> GetItemCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.ItemCategory.Where(itemcategory => itemcategory.Id == key));
        }

        // PUT odata/ItemCategory(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, ItemCategory itemcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != itemcategory.Id)
            {
                return BadRequest();
            }

            db.Entry(itemcategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemcategory);
        }

        // POST odata/ItemCategory
        public async Task<IHttpActionResult> Post(ItemCategory itemcategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ItemCategory.Add(itemcategory);
            await db.SaveChangesAsync();

            return Created(itemcategory);
        }

        // PATCH odata/ItemCategory(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ItemCategory> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ItemCategory itemcategory = await db.ItemCategory.FindAsync(key);
            if (itemcategory == null)
            {
                return NotFound();
            }

            patch.Patch(itemcategory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemcategory);
        }

        // DELETE odata/ItemCategory(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            ItemCategory itemcategory = await db.ItemCategory.FindAsync(key);
            if (itemcategory == null)
            {
                return NotFound();
            }

            db.ItemCategory.Remove(itemcategory);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemCategoryExists(int key)
        {
            return db.ItemCategory.Count(e => e.Id == key) > 0;
        }
    }
}
