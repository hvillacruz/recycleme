using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using RecycleMeDomainClasses;
using Microsoft.AspNet.Identity.EntityFramework;
using RecycleMeDataAccessLayer;

namespace RecycleMeOdataWebApi.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {

            return base.OnConnected();
        }



        public override Task OnDisconnected()
        {

            return base.OnDisconnected();
        }


        [Authorize]
        public void SendNotification(string message,string id)
        {
            using (UserManager<AspNetUsers> userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext())))
            {
                var user = userManager.Users.Where(a => a.Id == id).Select(a => a.UserName).FirstOrDefault();

                var excerpt = message.Length <= 30 ? message : message.Substring(0, 30) + "...";
                Clients.User(user).RecycleNotification(excerpt);

            }
        }

        [Authorize]
        public void MessageNotification(string message, string id)
        {
            using (UserManager<AspNetUsers> userManager = new UserManager<AspNetUsers>(new UserStore<AspNetUsers>(new RecycleMeContext())))
            {
                var user = userManager.Users.Where(a => a.Id == id).Select(a=>a.UserName).FirstOrDefault();

                var excerpt = message.Length <= 30 ? message : message.Substring(0, 30) + "...";
                Clients.User(user).MessageNotification(excerpt);

            }
        }


    }
}