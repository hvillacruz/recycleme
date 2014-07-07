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
    builder.EntitySet<Notification>("Notification");
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class NotificationController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/Notification
        [Queryable]
        public IQueryable<Notification> GetNotification()
        {
            return db.Notification;
        }

        // GET odata/Notification(5)
        [Queryable]
        public SingleResult<Notification> GetNotification([FromODataUri] long key)
        {
            return SingleResult.Create(db.Notification.Where(notification => notification.Id == key));
        }

        // PUT odata/Notification(5)
        public IHttpActionResult Put([FromODataUri] long key, Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != notification.Id)
            {
                return BadRequest();
            }

            db.Entry(notification).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(notification);
        }

        // POST odata/Notification
        public IHttpActionResult Post(Notification notification)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Notification.Add(notification);
            db.SaveChanges();

            return Created(notification);
        }

        // PATCH odata/Notification(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<Notification> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Notification notification = db.Notification.Find(key);
            if (notification == null)
            {
                return NotFound();
            }

            patch.Patch(notification);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(notification);
        }

        // DELETE odata/Notification(5)
        public IHttpActionResult Delete([FromODataUri] long key)
        {
            Notification notification = db.Notification.Find(key);
            if (notification == null)
            {
                return NotFound();
            }

            db.Notification.Remove(notification);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Notification(5)/Owner
        [Queryable]
        public SingleResult<User> GetOwner([FromODataUri] long key)
        {
            return SingleResult.Create(db.Notification.Where(m => m.Id == key).Select(m => m.Owner));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NotificationExists(long key)
        {
            return db.Notification.Count(e => e.Id == key) > 0;
        }
    }
}
