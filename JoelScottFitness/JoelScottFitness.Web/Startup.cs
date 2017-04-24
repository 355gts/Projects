using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JoelScottFitness.Web.Startup))]
namespace JoelScottFitness.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
