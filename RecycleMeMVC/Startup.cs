using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RecycleMeMVC.Startup))]
namespace RecycleMeMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
    }
}
