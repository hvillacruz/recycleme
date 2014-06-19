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
    builder.EntitySet<Trade>("Trade");
    builder.EntitySet<User>("Users"); 
    builder.EntitySet<Item>("Items"); 
    builder.EntitySet<TradeComment>("TradeComment"); 
    builder.EntitySet<TradeBuyerItem>("TradeBuyerItem"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TradeController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/Trade
        [Queryable]
        public IQueryable<Trade> GetTrade()
        {
            return db.Trade;
        }

        // GET odata/Trade(5)
        [Queryable]
        public SingleResult<Trade> GetTrade([FromODataUri] long key)
        {
            return SingleResult.Create(db.Trade.Where(trade => trade.Id == key));
        }

        // PUT odata/Trade(5)
        public IHttpActionResult Put([FromODataUri] long key, Trade trade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != trade.Id)
            {
                return BadRequest();
            }

            db.Entry(trade).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(trade);
        }

        // POST odata/Trade
        public IHttpActionResult Post(Trade trade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trade.Add(trade);
            db.SaveChanges();

            return Created(trade);
        }

        // PATCH odata/Trade(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<Trade> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Trade trade = db.Trade.Find(key);
            if (trade == null)
            {
                return NotFound();
            }

            patch.Patch(trade);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TradeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(trade);
        }

        // DELETE odata/Trade(5)
        public IHttpActionResult Delete([FromODataUri] long key)
        {
            Trade trade = db.Trade.Find(key);
            if (trade == null)
            {
                return NotFound();
            }

            db.Trade.Remove(trade);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Trade(5)/Buyer
        [Queryable]
        public SingleResult<User> GetBuyer([FromODataUri] long key)
        {
            return SingleResult.Create(db.Trade.Where(m => m.Id == key).Select(m => m.Buyer));
        }

        // GET odata/Trade(5)/Item
        [Queryable]
        public SingleResult<Item> GetItem([FromODataUri] long key)
        {
            return SingleResult.Create(db.Trade.Where(m => m.Id == key).Select(m => m.Item));
        }

        // GET odata/Trade(5)/Seller
        [Queryable]
        public SingleResult<User> GetSeller([FromODataUri] long key)
        {
            return SingleResult.Create(db.Trade.Where(m => m.Id == key).Select(m => m.Seller));
        }

        // GET odata/Trade(5)/TradeItem
        [Queryable]
        public IQueryable<TradeComment> GetTradeItem([FromODataUri] long key)
        {
            return db.Trade.Where(m => m.Id == key).SelectMany(m => m.TradeItem);
        }

        // GET odata/Trade(5)/Trades
        [Queryable]
        public IQueryable<TradeBuyerItem> GetTrades([FromODataUri] long key)
        {
            return db.Trade.Where(m => m.Id == key).SelectMany(m => m.Trades);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TradeExists(long key)
        {
            return db.Trade.Count(e => e.Id == key) > 0;
        }
    }
}
