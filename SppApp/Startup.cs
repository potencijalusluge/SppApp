using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SppApp.Startup))]
namespace SppApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
