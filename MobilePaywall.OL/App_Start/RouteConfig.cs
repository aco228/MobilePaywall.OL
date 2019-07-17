using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MobilePaywall.OL
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
      
      routes.MapRoute(
        name: "Database",
        url: "Database",
        defaults: new { controller = "Database", action = "Index", id = UrlParameter.Optional }
      );
      
      routes.MapRoute(
        name: "Report",
        url: "Report/{uid}",
        defaults: new { controller = "Database", action = "Report", uid = UrlParameter.Optional }
      );

      routes.MapRoute(
        name: "Logout",
        url: "Logout",
        defaults: new { controller = "Login", action = "Logout", id = UrlParameter.Optional }
      );
    
      routes.MapRoute(
          name: "Default",
          url: "{controller}/{action}/{id}",
          defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
      );
      
    }
  }
}
