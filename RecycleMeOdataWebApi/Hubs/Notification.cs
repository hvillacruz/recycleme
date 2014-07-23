using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

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
        public void SendNotification(string message)
        {
            var excerpt = message.Length <= 10 ? message : message.Substring(0, 10) + "...";
            Clients.All.RecycleNotification(excerpt);
        }

    }
}