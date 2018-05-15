using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(wpd2coursework.Startup))]
namespace wpd2coursework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
