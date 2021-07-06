using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigSchool1.Startup))]
namespace BigSchool1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
