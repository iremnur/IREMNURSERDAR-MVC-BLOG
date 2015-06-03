using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IremNurSerdarBlog.Startup))]
namespace IremNurSerdarBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
