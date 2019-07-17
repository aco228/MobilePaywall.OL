using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MobilePaywall.OL.Startup))]
namespace MobilePaywall.OL
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);
      app.MapSignalR();
    }
  }
}
