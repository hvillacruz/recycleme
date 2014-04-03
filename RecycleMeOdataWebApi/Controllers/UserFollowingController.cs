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
    builder.EntitySet<UserFollowing>("UserFollowing");
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UserFollowingController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/UserFollowing
        [Queryable]
        public IQueryable<UserFollowing> GetUserFollowing()
        {
            return db.UserFollowing;
        }

        // GET odata/UserFollowing(5)
        [Queryable]
        public SingleResult<UserFollowing> GetUserFollowing([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserFollowing.Where(userfollowing => userfollowing.Id == key));
        }

        // PUT odata/UserFollowing(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, UserFollowing userfollowing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != userfollowing.Id)
            {
                return BadRequest();
            }

            db.Entry(userfollowing).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFollowingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userfollowing);
        }

        // POST odata/UserFollowing
        public async Task<IHttpActionResult> Post(UserFollowing userfollowing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserFollowing.Add(userfollowing);
            await db.SaveChangesAsync();

            return Created(userfollowing);
        }

        // PATCH odata/UserFollowing(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<UserFollowing> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserFollowing userfollowing = await db.UserFollowing.FindAsync(key);
            if (userfollowing == null)
            {
                return NotFound();
            }

            patch.Patch(userfollowing);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFollowingExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userfollowing);
        }

        // DELETE odata/UserFollowing(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            UserFollowing userfollowing = await db.UserFollowing.FindAsync(key);
            if (userfollowing == null)
            {
                return NotFound();
            }

            db.UserFollowing.Remove(userfollowing);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/UserFollowing(5)/Following
        [Queryable]
        public SingleResult<User> GetFollowing([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserFollowing.Where(m => m.Id == key).Select(m => m.Following));
        }

        // GET odata/UserFollowing(5)/FollowingUser
        [Queryable]
        public SingleResult<User> GetFollowingUser([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserFollowing.Where(m => m.Id == key).Select(m => m.FollowingUser));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserFollowingExists(Guid key)
        {
            return db.UserFollowing.Count(e => e.Id == key) > 0;
        }
    }
}
