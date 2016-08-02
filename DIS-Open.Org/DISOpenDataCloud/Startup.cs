using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DISOpenDataCloud.Startup))]
namespace DISOpenDataCloud
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
