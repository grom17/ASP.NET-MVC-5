using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SimpleStudentsWebsite.Startup))]
namespace SimpleStudentsWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
