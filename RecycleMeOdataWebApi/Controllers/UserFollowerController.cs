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
    builder.EntitySet<UserFollower>("UserFollower");
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UserFollowerController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/UserFollower
        [Queryable]
        public IQueryable<UserFollower> GetUserFollower()
        {
            return db.UserFollower;
        }

        // GET odata/UserFollower(5)
        [Queryable]
        public SingleResult<UserFollower> GetUserFollower([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserFollower.Where(userfollower => userfollower.Id == key));
        }

        // PUT odata/UserFollower(5)
        public async Task<IHttpActionResult> Put([FromODataUri] Guid key, UserFollower userfollower)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != userfollower.Id)
            {
                return BadRequest();
            }

            db.Entry(userfollower).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFollowerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userfollower);
        }

        // POST odata/UserFollower
        public async Task<IHttpActionResult> Post(UserFollower userfollower)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserFollower.Add(userfollower);
            await db.SaveChangesAsync();

            return Created(userfollower);
        }

        // PATCH odata/UserFollower(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<UserFollower> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserFollower userfollower = await db.UserFollower.FindAsync(key);
            if (userfollower == null)
            {
                return NotFound();
            }

            patch.Patch(userfollower);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFollowerExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(userfollower);
        }

        // DELETE odata/UserFollower(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            UserFollower userfollower = await db.UserFollower.FindAsync(key);
            if (userfollower == null)
            {
                return NotFound();
            }

            db.UserFollower.Remove(userfollower);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/UserFollower(5)/Follower
        [Queryable]
        public SingleResult<User> GetFollower([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserFollower.Where(m => m.Id == key).Select(m => m.Follower));
        }

        // GET odata/UserFollower(5)/User
        [Queryable]
        public SingleResult<User> GetUser([FromODataUri] Guid key)
        {
            return SingleResult.Create(db.UserFollower.Where(m => m.Id == key).Select(m => m.User));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserFollowerExists(Guid key)
        {
            return db.UserFollower.Count(e => e.Id == key) > 0;
        }
    }
}
