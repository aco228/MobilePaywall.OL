using System.Web;
using System.Web.Optimization;

namespace MobilePaywall.OL
{
  public class BundleConfig
  {
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
      BundleTable.EnableOptimizations = false;

      bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css",
                "~/Content/Visual.css"));
      bundles.Add(new ScriptBundle("~/js").Include("~/Scripts/System.js")
        .Include("~/Scripts/Visual.js"));


      bundles.Add(new StyleBundle("~/Database/css").Include("~/Content/Database/main.css")
        .Include("~/Content/Database/mainMobile.css"));

      bundles.Add(new ScriptBundle("~/Database/js").Include("~/Scripts/Database/WindowManager.js")
        .Include("~/Scripts/Database/DatabaseConfigurationManager.js")
        .Include("~/Scripts/Database/DatabaseManager.js"));

      bundles.Add(new StyleBundle("~/Error/css").Include("~/Content/Error.css"));

      bundles.Add(new StyleBundle("~/Login/css").Include("~/Content/Login.css"));
      bundles.Add(new ScriptBundle("~/Login/js").Include("~/Scripts/Login.js"));


      bundles.Add(new StyleBundle("~/Report/css").Include("~/Content/Database/report.css"));
      bundles.Add(new ScriptBundle("~/Report/js").Include("~/Scripts/Database/ReportManager.js"));
      bundles.Add(new ScriptBundle("~/Highlight/js").Include("~/Scripts/highlight.pack.js"));
      bundles.Add(new StyleBundle("~/Highlight/css").Include("~/Content/hightlight/default.css").Include("~/Content/hightlight/railscasts.css"));
      bundles.Add(new StyleBundle("~/Statistic/css").Include("~/Content/Statistic/main.css"));
      bundles.Add(new ScriptBundle("~/Statistic/js").Include("~/Scripts/Statistic/Statistic.js"));
      bundles.Add(new StyleBundle("~/QuickReport/css").Include("~/Content/Statistic/QuickReport.css"));

      bundles.Add(new StyleBundle("~/Kiwi/css").Include("~/Content/Kiwi/main.css"));
      bundles.Add(new ScriptBundle("~/Kiwi/js").Include("~/Scripts/Kiwi/main.js"));

      bundles.Add(new StyleBundle("~/Cashflow/css").Include("~/Content/Statistic/Cashflow.css"));

      bundles.Add(new ScriptBundle("~/Mobile/css").Include("~/Content/Mobile/main.css"));
      bundles.Add(new ScriptBundle("~/Mobile/js").Include("~/Scripts/Mobile/MobileManager.js"));

    }
  }
}
