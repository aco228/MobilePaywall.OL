using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MobilePaywall.OL
{
  public class OLApplication : System.Web.HttpApplication
  {

    public static List<Country> Country = null;
    public static List<MobileOperator> MobileOperator = null;

    protected void Application_Start()
    {
      MobilePaywall.Data.Sql.Database dummy = null;
      Senti.Data.DataLayerRuntime r = new Senti.Data.DataLayerRuntime();

      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

      OLApplication.Country = Data.Country.CreateManager().Load();
      OLApplication.MobileOperator = Data.MobileOperator.CreateManager().Load();
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      Exception exception = Server.GetLastError();
      Server.ClearError();
      
      int a = 1;
    }

    void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
      Exception exception = e.ExceptionObject as Exception;
      int a = 1;
    }


    
  }
}
