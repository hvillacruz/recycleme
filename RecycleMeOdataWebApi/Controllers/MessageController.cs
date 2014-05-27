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
    builder.EntitySet<Message>("Message");
    builder.EntitySet<User>("Users"); 
    config.Routes.MapODataRoute("odata", "odata", builder.GetEdmModel());
    */
    public class MessageController : ODataController
    {
        private RecycleMeContext db = new RecycleMeContext();

        // GET odata/Message
        [Queryable]
        public IQueryable<Message> GetMessage()
        {
            return db.Message;
        }

        // GET odata/Message(5)
        [Queryable]
        public SingleResult<Message> GetMessage([FromODataUri] long key)
        {
            return SingleResult.Create(db.Message.Where(message => message.Id == key));
        }

        // PUT odata/Message(5)
        public IHttpActionResult Put([FromODataUri] long key, Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != message.Id)
            {
                return BadRequest();
            }

            db.Entry(message).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(message);
        }

        // POST odata/Message
        public IHttpActionResult Post(Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Message.Add(message);
            db.SaveChanges();

            return Created(message);
        }

        // PATCH odata/Message(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] long key, Delta<Message> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Message message = db.Message.Find(key);
            if (message == null)
            {
                return NotFound();
            }

            patch.Patch(message);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(message);
        }

        // DELETE odata/Message(5)
        public IHttpActionResult Delete([FromODataUri] long key)
        {
            Message message = db.Message.Find(key);
            if (message == null)
            {
                return NotFound();
            }

            db.Message.Remove(message);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Message(5)/Receiver
        [Queryable]
        public SingleResult<User> GetReceiver([FromODataUri] long key)
        {
            return SingleResult.Create(db.Message.Where(m => m.Id == key).Select(m => m.Receiver));
        }

        // GET odata/Message(5)/Sender
        [Queryable]
        public SingleResult<User> GetSender([FromODataUri] long key)
        {
            return SingleResult.Create(db.Message.Where(m => m.Id == key).Select(m => m.Sender));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(long key)
        {
            return db.Message.Count(e => e.Id == key) > 0;
        }
    }
}
