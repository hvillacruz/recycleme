using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace RecycleMeHub.Hubs
{
    public class PostHub : Hub
    {
        public void Hello()
        {
            Clients.Caller.addMessage("this is henry");
        }
    }
}