using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sales01.Backend.Startup))]
namespace Sales01.Backend
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
