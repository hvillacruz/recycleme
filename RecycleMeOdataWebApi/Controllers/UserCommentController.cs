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
    builder.EntitySet<UserComment>("UserComment");
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class UserCommentController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/UserComment
        [Queryable]
        public IQueryable<UserComment> GetUserComment()
        {
            return db.UserComment;
        }

        // GET odata/UserComment(5)
        [Queryable]
        public SingleResult<UserComment> GetUserComment([FromODataUri] long key)
        {
            return SingleResult.Create(db.UserComment.Where(usercomment => usercomment.Id == key));
        }

        // PUT odata/UserComment(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, UserComment usercomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != usercomment.Id)
            {
                return BadRequest();
            }

            db.Entry(usercomment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(usercomment);
        }

        // POST odata/UserComment
        public async Task<IHttpActionResult> Post(UserComment usercomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserComment.Add(usercomment);
            await db.SaveChangesAsync();

            return Created(usercomment);
        }

        // PATCH odata/UserComment(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<UserComment> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserComment usercomment = await db.UserComment.FindAsync(key);
            if (usercomment == null)
            {
                return NotFound();
            }

            patch.Patch(usercomment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(usercomment);
        }

        // DELETE odata/UserComment(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            UserComment usercomment = await db.UserComment.FindAsync(key);
            if (usercomment == null)
            {
                return NotFound();
            }

            db.UserComment.Remove(usercomment);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/UserComment(5)/CommentedUser
        [Queryable]
        public SingleResult<User> GetCommentedUser([FromODataUri] long key)
        {
            return SingleResult.Create(db.UserComment.Where(m => m.Id == key).Select(m => m.CommentedUser));
        }

        // GET odata/UserComment(5)/Commenter
        [Queryable]
        public SingleResult<User> GetCommenter([FromODataUri] long key)
        {
            return SingleResult.Create(db.UserComment.Where(m => m.Id == key).Select(m => m.Commenter));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserCommentExists(long key)
        {
            return db.UserComment.Count(e => e.Id == key) > 0;
        }
    }
}
