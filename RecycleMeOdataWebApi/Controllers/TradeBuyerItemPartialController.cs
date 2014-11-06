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
using System.Threading.Tasks;

namespace RecycleMeOdataWebApi.Controllers
{
   
    public partial class TradeBuyerItemController : ODataController
    {


        [HttpPost]
        public IHttpActionResult TradeBuyerItemDelete(ODataActionParameters parameters)
        {

            var key = long.Parse(parameters["TradeId"].ToString());
            var tradebuyeritem = db.TradeBuyerItem.Where(a => a.TradeId == key).ToList().Select(b => b);
            if (tradebuyeritem == null)
            {
                return NotFound();
            }

            foreach (var item in tradebuyeritem)
            {

                db.TradeBuyerItem.Remove(item);
                db.Entry(item).State = EntityState.Deleted;
                db.SaveChanges();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
