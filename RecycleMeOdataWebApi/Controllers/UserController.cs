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
    builder.EntitySet<User>("User");
    builder.EntitySet<Item>("Item"); 
    builder.EntitySet<UserComment>("UserComment"); 
    builder.EntitySet<UserFollower>("UserFollower"); 
    builder.EntitySet<UserFollowing>("UserFollowing"); 
    builder.EntitySet<ItemComment>("ItemComment"); 
    builder.EntitySet<ItemFollowers>("ItemFollowers"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UserController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/User
        [Queryable]
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
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

        // GET odata/User(5)/Items
        [Queryable]
        public IQueryable<Item> GetItems([FromODataUri] string key)
        {
            var items =db.Users.Where(m => m.UserId == key).SelectMany(m => m.Items);
            return items;

        }

        // GET odata/User(5)/UserCommented
        [Queryable]
        public IQueryable<UserComment> GetUserCommented([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserCommented);
        }

        // GET odata/User(5)/UserCommenter
        [Queryable]
        public IQueryable<UserComment> GetUserCommenter([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserCommenter);
        }

        // GET odata/User(5)/UserFollowers
        [Queryable]
        public IQueryable<UserFollower> GetUserFollowers([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserFollowers);
        }

        // GET odata/User(5)/UserFollowerUsers
        [Queryable]
        public IQueryable<UserFollower> GetUserFollowerUsers([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserFollowerUsers);
        }

        // GET odata/User(5)/UserFollowing
        [Queryable]
        public IQueryable<UserFollowing> GetUserFollowing([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserFollowing);
        }

        // GET odata/User(5)/UserFollowingUsers
        [Queryable]
        public IQueryable<UserFollowing> GetUserFollowingUsers([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserFollowingUsers);
        }

        // GET odata/User(5)/UserItemCommenter
        [Queryable]
        public IQueryable<ItemComment> GetUserItemCommenter([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserItemCommenter);
        }

        // GET odata/User(5)/UserItemFollowers
        [Queryable]
        public IQueryable<ItemFollowers> GetUserItemFollowers([FromODataUri] string key)
        {
            return db.Users.Where(m => m.UserId == key).SelectMany(m => m.UserItemFollowers);
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
