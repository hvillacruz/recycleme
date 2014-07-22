using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace RecycleMeOdataWebApi
{
    [Authorize]
    public class AuthorizeEchoHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.Caller.hubReceived("Welcome " + Context.User.Identity.Name + "!");
            //Clients.User("signalr").hubReceived("you rock");
        }

        public void Echo(string value)
        {
            Clients.Caller.hubReceived(value);
        }
    }
}
