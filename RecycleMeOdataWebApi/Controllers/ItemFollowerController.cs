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
    builder.EntitySet<ItemFollowers>("ItemFollower");
    builder.EntitySet<Item>("Items"); 
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ItemFollowerController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/ItemFollower
        [Queryable]
        public IQueryable<ItemFollowers> GetItemFollower()
        {
            return db.ItemFollowers;
        }

        // GET odata/ItemFollower(5)
        [Queryable]
        public SingleResult<ItemFollowers> GetItemFollowers([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemFollowers.Where(itemfollowers => itemfollowers.Id == key));
        }

        // PUT odata/ItemFollower(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, ItemFollowers itemfollowers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != itemfollowers.Id)
            {
                return BadRequest();
            }

            db.Entry(itemfollowers).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemFollowersExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemfollowers);
        }

        // POST odata/ItemFollower
        public async Task<IHttpActionResult> Post(ItemFollowers itemfollowers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ItemFollowers.Add(itemfollowers);
            await db.SaveChangesAsync();

            return Created(itemfollowers);
        }

        // PATCH odata/ItemFollower(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<ItemFollowers> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ItemFollowers itemfollowers = await db.ItemFollowers.FindAsync(key);
            if (itemfollowers == null)
            {
                return NotFound();
            }

            patch.Patch(itemfollowers);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemFollowersExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemfollowers);
        }

        // DELETE odata/ItemFollower(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            ItemFollowers itemfollowers = await db.ItemFollowers.FindAsync(key);
            if (itemfollowers == null)
            {
                return NotFound();
            }

            db.ItemFollowers.Remove(itemfollowers);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/ItemFollower(5)/FollowedItem
        [Queryable]
        public SingleResult<Item> GetFollowedItem([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemFollowers.Where(m => m.Id == key).Select(m => m.FollowedItem));
        }

        // GET odata/ItemFollower(5)/Follower
        [Queryable]
        public SingleResult<User> GetFollower([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemFollowers.Where(m => m.Id == key).Select(m => m.Follower));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemFollowersExists(long key)
        {
            return db.ItemFollowers.Count(e => e.Id == key) > 0;
        }
    }
}
