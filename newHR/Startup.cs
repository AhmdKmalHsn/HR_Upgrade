using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(newHR.Startup))]
namespace newHR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
