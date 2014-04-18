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
    builder.EntitySet<ItemComment>("ItemComment");
    builder.EntitySet<Item>("Items"); 
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class ItemCommentController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/ItemComment
        [Queryable]
        public IQueryable<ItemComment> GetItemComment()
        {
            return db.ItemComment;
        }

        // GET odata/ItemComment(5)
        [Queryable]
        public SingleResult<ItemComment> GetItemComment([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemComment.Where(itemcomment => itemcomment.Id == key));
        }

        // PUT odata/ItemComment(5)
        public async Task<IHttpActionResult> Put([FromODataUri] long key, ItemComment itemcomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != itemcomment.Id)
            {
                return BadRequest();
            }

            db.Entry(itemcomment).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemcomment);
        }

        // POST odata/ItemComment
        public async Task<IHttpActionResult> Post(ItemComment itemcomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ItemComment.Add(itemcomment);
            await db.SaveChangesAsync();

            return Created(itemcomment);
        }

        // PATCH odata/ItemComment(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] long key, Delta<ItemComment> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ItemComment itemcomment = await db.ItemComment.FindAsync(key);
            if (itemcomment == null)
            {
                return NotFound();
            }

            patch.Patch(itemcomment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(itemcomment);
        }

        // DELETE odata/ItemComment(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] long key)
        {
            ItemComment itemcomment = await db.ItemComment.FindAsync(key);
            if (itemcomment == null)
            {
                return NotFound();
            }

            db.ItemComment.Remove(itemcomment);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/ItemComment(5)/CommentedItem
        [Queryable]
        public SingleResult<Item> GetCommentedItem([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemComment.Where(m => m.Id == key).Select(m => m.CommentedItem));
        }

        // GET odata/ItemComment(5)/Commenter
        [Queryable]
        public SingleResult<User> GetCommenter([FromODataUri] long key)
        {
            return SingleResult.Create(db.ItemComment.Where(m => m.Id == key).Select(m => m.Commenter));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemCommentExists(long key)
        {
            return db.ItemComment.Count(e => e.Id == key) > 0;
        }
    }
}
