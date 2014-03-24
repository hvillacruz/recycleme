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
using System.Web.Http.OData.Query;

namespace RecycleMeOdataWebApi.Controllers
{
    /*
    To add a route for this controller, merge these statements into the Register method of the WebApiConfig class. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using RecycleMeDomainClasses;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<User>("User");
    builder.EntitySet<UserComment>("UserComment"); 
    builder.EntitySet<UserFollower>("UserFollower"); 
    builder.EntitySet<UserFollowing>("UserFollowing"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UserController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/User
         [Queryable(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<User> GetUser()
        {
            return db.Users;
        }

        // GET odata/User(5)
        [Queryable]
        public SingleResult<User> GetUser([FromODataUri] string key)
        {
            return SingleResult.Create(db.Users.Where(user => user.UserId == key));
        }

        // PUT odata/User(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(user);
        }

        // POST odata/User
        public async Task<IHttpActionResult> Post(User user)
        {
            user.UserId = Guid.NewGuid().ToString();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(user);
        }

        // PATCH odata/User(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<User> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await db.Users.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            patch.Patch(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(user);
        }

        // DELETE odata/User(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            User user = await db.Users.FindAsync(key);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/User(5)/UserComments
        [Queryable]
        public IQueryable<UserComment> GetUserComments([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserComments);
        }

        // GET odata/User(5)/UserFollowers
        [Queryable]
        public IQueryable<UserFollower> GetUserFollowers([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserFollowers);
        }

        // GET odata/User(5)/UserFollowing
        [Queryable]
        public IQueryable<UserFollowing> GetUserFollowing([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserFollowing);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string key)
        {
            return db.Users.Count(e => e.UserId == key) > 0;
        }
    }
}
