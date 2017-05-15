using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Einvoice_Customer.Startup))]
namespace Einvoice_Customer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
