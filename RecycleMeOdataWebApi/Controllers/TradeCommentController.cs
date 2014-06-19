using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    builder.EntitySet<TradeComment>("TradeComment");
    builder.EntitySet<User>("Users"); 
    builder.EntitySet<Trade>("Trade"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TradeCommentController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/TradeComment
        [Queryable]
        public IQueryable<TradeComment> GetTradeComment()
        {
            return db.TradeComment;
        }

        // GET odata/TradeComment(5)
        [Queryable]
        public SingleResult<TradeComment> GetTradeComment([FromODataUri] long key)
        {
            return SingleResult.Create(db.TradeComment.Where(tradecomment => tradecomment.Id == key));
        }

        // PUT odata/TradeComment(5)
        public IHttpActionResult Put([FromODataUri] long key, TradeComment tradecomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != tradecomment.Id)
            {
                return BadRequest();
            }

            db.Entry(tradecomment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tradecomment);
        }

        // POST odata/TradeComment
        public IHttpActionResult Post(TradeComment tradecomment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeComment.Add(tradecomment);
            db.SaveChanges();

            return Created(tradecomment);
        }

        // PATCH odata/TradeComment(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<TradeComment> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TradeComment tradecomment = db.TradeComment.Find(key);
            if (tradecomment == null)
            {
                return NotFound();
            }

            patch.Patch(tradecomment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeCommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tradecomment);
        }

        // DELETE odata/TradeComment(5)
        public IHttpActionResult Delete([FromODataUri] long key)
        {
            TradeComment tradecomment = db.TradeComment.Find(key);
            if (tradecomment == null)
            {
                return NotFound();
            }

            db.TradeComment.Remove(tradecomment);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/TradeComment(5)/TradeCommenter
        [Queryable]
        public SingleResult<User> GetTradeCommenter([FromODataUri] long key)
        {
            return SingleResult.Create(db.TradeComment.Where(m => m.Id == key).Select(m => m.TradeCommenter));
        }

        // GET odata/TradeComment(5)/TradeItemCommented
        [Queryable]
        public SingleResult<Trade> GetTradeItemCommented([FromODataUri] long key)
        {
            return SingleResult.Create(db.TradeComment.Where(m => m.Id == key).Select(m => m.TradeItemCommented));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradeCommentExists(long key)
        {
            return db.TradeComment.Count(e => e.Id == key) > 0;
        }
    }
}
