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
    builder.EntitySet<TradeBuyerItem>("TradeBuyerItem");
    builder.EntitySet<Item>("Items"); 
    builder.EntitySet<Trade>("Trade"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public partial class TradeBuyerItemController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/TradeBuyerItem
        [Queryable]
        public IQueryable<TradeBuyerItem> GetTradeBuyerItem()
        {
            return db.TradeBuyerItem;
        }

        // GET odata/TradeBuyerItem(5)
        [Queryable]
        public SingleResult<TradeBuyerItem> GetTradeBuyerItem([FromODataUri] long key)
        {
            return SingleResult.Create(db.TradeBuyerItem.Where(tradebuyeritem => tradebuyeritem.Id == key));
        }

        // PUT odata/TradeBuyerItem(5)
        public IHttpActionResult Put([FromODataUri] long key, TradeBuyerItem tradebuyeritem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != tradebuyeritem.Id)
            {
                return BadRequest();
            }

            db.Entry(tradebuyeritem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeBuyerItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tradebuyeritem);
        }

        // POST odata/TradeBuyerItem
        public IHttpActionResult Post(TradeBuyerItem tradebuyeritem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TradeBuyerItem.Add(tradebuyeritem);
            db.SaveChanges();

            return Created(tradebuyeritem);
        }

        // PATCH odata/TradeBuyerItem(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<TradeBuyerItem> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TradeBuyerItem tradebuyeritem = db.TradeBuyerItem.Find(key);
            if (tradebuyeritem == null)
            {
                return NotFound();
            }

            patch.Patch(tradebuyeritem);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeBuyerItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(tradebuyeritem);
        }

        // DELETE odata/TradeBuyerItem(5)
        public IHttpActionResult Delete([FromODataUri] long key)
        {
            TradeBuyerItem tradebuyeritem = db.TradeBuyerItem.Find(key);
            if (tradebuyeritem == null)
            {
                return NotFound();
            }

            db.TradeBuyerItem.Remove(tradebuyeritem);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/TradeBuyerItem(5)/Item
        [Queryable]
        public SingleResult<Item> GetItem([FromODataUri] long key)
        {
            return SingleResult.Create(db.TradeBuyerItem.Where(m => m.Id == key).Select(m => m.Item));
        }

        // GET odata/TradeBuyerItem(5)/Trade
        [Queryable]
        public SingleResult<Trade> GetTrade([FromODataUri] long key)
        {
            return SingleResult.Create(db.TradeBuyerItem.Where(m => m.Id == key).Select(m => m.Trade));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradeBuyerItemExists(long key)
        {
            return db.TradeBuyerItem.Count(e => e.Id == key) > 0;
        }
    }
}
