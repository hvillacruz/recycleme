using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;

namespace RecycleMeHub.Hubs
{
    public class PostHub : Hub
    {
        public void Hello(string userName)
        {
            //var identity = Thread.CurrentPrincipal.Identity;
            //var request = Context.Request;
            //Clients.Client(Context.ConnectionId).sayhello("Hello " + identity.Name);

            //string userName = Clients.CallerState.userName;
            //string computerName = Clients.CallerState.computerName;
            //Clients.Others.addContosoChatMessageToPage(data, userName, computerName);

            Clients.Client("9bce472a-1226-4265-ba4b-d761052650ec").addMessage("hello henry");
            //Clients.All.addMessage(userName + " is in the house");
        }


        public void SendToUser(string userId, string value)
        {

            //System.Collections.Generic.IDictionary<string, Cookie> cookies = Context.Request.Cookies;
            //Console.Write(cookies);
            //Clients.User(Context.User.Identity.Name).message(value);
            Clients.User(userId).message(value);
        }
    }
}