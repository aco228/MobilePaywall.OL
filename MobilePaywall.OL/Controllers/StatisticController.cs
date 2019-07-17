using MobilePaywall.Ol.Core;
using MobilePaywall.Ol.Core.Data;
using MobilePaywall.Ol.Core.Managers;
using MobilePaywall.Ol.Core.Models;
using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class StatisticController : PaywallController
  {

    public ActionResult Index(string from, string to)
    {
      if(this.CheckDate(from) || this.CheckDate(to))
        return Content(string.Format("<b class=\"message_error\">Internal error. '{0}'</b>", "Wrong date format"));

      return null;
    }

    public ActionResult Timeline(EntranceTableData data)
    {
      if (data.Validation())
        return Content(string.Format("<b class=\"message_error\">Internal error. '{0}'</b>", data.ErrorMessage));

      StatisticManager manager = new StatisticManager();
      TimelineModel model = new TimelineModel();
      model.Data = manager.LoadTimeline(data);

      return View("~/Views/Statistic/Timeline.cshtml", model );
    }

    public ActionResult Cashflow(EntranceTableData data)
    {
      if (data.Validation())
        return Content(string.Format("<b class=\"message_error\">Internal error. '{0}'</b>", data.ErrorMessage));

      CashflowLogManager manager = new CashflowLogManager();
      CashflowStatisticModel model = new CashflowStatisticModel(manager.Query(data, true));
      return View("~/Views/Statistic/CashflowReport.cshtml", model);
    }

    public ActionResult QuickReport(EntranceTableData data)
    {
      if (data.Validation())
        return Content(string.Format("<b class=\"message_error\">Internal error. '{0}'</b>", data.ErrorMessage));

      //StatisticManager manager = new StatisticManager();
      //QuickReportModel model = new QuickReportModel(manager.LoadQuickReports(data));
      return View("~/Views/Statistic/QuickReport.cshtml", new QuickReportModel(null));
    }

    // SUMMARY: Method for async load for quicreports
    public string LoadUserSessions(EntranceTableData data)
    {
      if (data.Validation())
        return string.Format("- validation error -");

      return JsonObjectHelper.Json(new StatisticManager().LoadUserSessions(data));
    }

    public string LoadPaymentReqests(EntranceTableData data)
    {
      if (data.Validation())
        return string.Format("- validation error -");

      return JsonObjectHelper.Json(new StatisticManager().LoadPaymentRequests(data));
    }

    public string LoadPayments(EntranceTableData data)
    {
      if (data.Validation())
        return string.Format("- validation error -");

      return JsonObjectHelper.Json(new StatisticManager().LoadPayments(data));
    }

    public string LoadTransactions(EntranceTableData data)
    {
      if (data.Validation())
        return string.Format("- validation error -");

      return JsonObjectHelper.Json(new StatisticManager().LoadTransactions(data));
    }
    
    private bool CheckDate(string data)
    {
      DateTime _temp;
      if (!DateTime.TryParse(data, out _temp))
        return true;
      return false;
    }

  }
}