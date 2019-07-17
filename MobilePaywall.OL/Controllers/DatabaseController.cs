using MobilePaywall.Direct;
using MobilePaywall.Ol.Core;
using MobilePaywall.Ol.Core.Data;
using MobilePaywall.Ol.Core.Managers;
using MobilePaywall.Ol.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class DatabaseController : PaywallController
  {

    // GET: Index
    public ActionResult Index()
    {
      ViewBag.Username = this.HttpContext.Request.Cookies["uid"].Value.ToString();
      ViewBag.AndroidSessionID = Request["aid"] != null ? Request["aid"] : null;

      return View("~/Views/Database/Index.cshtml", new ModelBase() { });
    }

    // SUMMARY: Load entrance table data
    public ActionResult Load(EntranceTableData data)
    {
      if(data.Validation())
        return Content(string.Format("<b class=\"message_error\">Internal error. '{0}'</b>", data.ErrorMessage));

      EntranceTableManager entranceTableManager = new EntranceTableManager();
      entranceTableManager.Query(data); 

      return View("~/Views/Database/_Data.cshtml", new DataTableModel(entranceTableManager));
    }

    public ActionResult LoadNew(EntranceTableData data)
    {
      if (data.UseSequentialSearch)
        return this.LoadNewSubsequental(data);

      if (data.Validation())
        return Content(string.Format("<b class=\"message_error\">Internal error. '{0}'</b>", data.ErrorMessage));

      EntranceTableManager entranceTableManager = new EntranceTableManager();
      
      DataTableModel model = new DataTableModel(entranceTableManager, 
        (!data.AndroidClientSession.HasValue ?
        entranceTableManager.QueryNew(data) : 
        entranceTableManager.QueryNewAndroidSession(data)));

      return View(string.Format("~/Views/Database/{0}.cshtml", data.ReturnView), model);
    }

    public ActionResult LoadNewSubsequental(EntranceTableData data)
    {
      if (data.Validation())
        return Content(string.Format("<b class=\"message_error\">Internal error. '{0}'</b>", data.ErrorMessage));

      EntranceTableManager entranceTableManager = new EntranceTableManager();
      DataTableModel model = new DataTableModel(entranceTableManager);

      return View("~/Views/Database/_DataNewSequential.cshtml", model);
    }

    // SUMMARY: Get .csv from for specific entrance table data
    public FileContentResult Csv(EntranceTableData data)
    {
      if (data.Validation())
        return null;

      EntranceTableManager entranceTableManager = new EntranceTableManager();
      entranceTableManager.Query(data);
      return File(new System.Text.UTF8Encoding().GetBytes(entranceTableManager.ConvertToCvs()), "text/csv", "database.csv");
    }

    /*
     *  REPORT
     */


    // SUMMARY: Load report for specific click
    public ActionResult Report(string uid)
    {
      int _userSessionID = -1;
      if(!Int32.TryParse(uid, out _userSessionID))
      {
        ErrorModel errorModel = new ErrorModel() { Title = "Fatal error", Description = "Could not parse UID" };
        return View("~/Views/Error.cshtml", errorModel);
      }

      MobilePaywall.Direct.MobilePaywallDirect database = new Direct.MobilePaywallDirect();
      DirectContainer userSessionCont = database.LoadContainer(@"SELECT UserSessionID, UserSessionGuid, EntranceUrl, IPAddress, Created 
                                                                 FROM MobilePaywall.core.UserSession WHERE UserSessionID=" + _userSessionID);

      if(!userSessionCont.HasValue)
      {
        ErrorModel errorModel = new ErrorModel() { Title = "Fatal error", Description = "UserSession could not be loaded" };
        return View("~/Views/Error.cshtml", errorModel);
      }


      ReportModel model = new ReportModel()
      {
        PaywallGuid = userSessionCont.GetString("UserSessionGuid"),
        UserSessionID = uid,
        Pxid = "",
        Date = userSessionCont.GetString("Created"),
        IP = userSessionCont.GetString("IPAddress"),
        HtmlTitle = "Report : " + uid
      };

      return View("~/Views/Report/Index.cshtml", model);
    }

		// Entrance from paywall test application 3g proxy
		public ActionResult FromPaywallTestApplication()
		{
			string pxid = Request["pxidfa"] != null ? Request["pxidfa"].ToString() : string.Empty;
			if (string.IsNullOrEmpty(pxid))
				return this.Content("No args");
			int? sid = MobilePaywallDirect.Instance.LoadInt(string.Format("SELECT TOP 1 UserSessionID FROM MobilePaywall.core.UserSession WHERE EntranceUrl LIKE '%pxidfa={0}%' ORDER BY UserSessionID DESC;", pxid));
			if (!sid.HasValue)
				return this.Content("no sid");
			return this.Redirect("/report/" + sid.Value);
		}
    
    // SUMMARY: Report -> Informations about click
    public ActionResult GetInformations(WebLogTableData data)
    {
      InformationManager manager = new InformationManager();
      manager.Query(data);
      return View("~/Views/Report/_LogTable.cshtml", new LogModel { Informations = manager.Result, InformationsLabel = "info" });
    }

    // SUMMARY: Report -> Old paywall report data from the Weblog
    public ActionResult GetPaywallLog(WebLogTableData data)
    {
      WebLogManager manager = new WebLogManager();
      manager.Query(data);
      return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
    }

    // SUMMARY: Report -> New system for loading paywall reports from WebLog
    public ActionResult GetPaywallLog2(WebLogTableData data)
    {
      WebLogManager manager = new WebLogManager();
      manager.Query2(data);
      return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
    }
    
    // SUMMARY: Report -> Load paywall logs from Log database
    public ActionResult GetPaywallLogDatabase(WebLogTableData data)
    {
      WebLogManager manager = new WebLogManager();
      manager.QueryLogDatabase(data);
      return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
    }

    // SUMMARY: Report -> Get cashflow logs for specific log
    public ActionResult GetCashflowLog(WebLogTableData data)
    {
      MobilePaywallDirect db = MobilePaywallDirect.Instance;
      CashflowLogManager manager = new CashflowLogManager();
      
      Guid? subscriptionRequestGuid = db.LoadGuid("SELECT ExternalPaymentRequestGuid FROM MobilePaywall.core.PaymentRequest WHERE ExternalPaymentRequestGuid IS NOT NULL AND UserSessionID=" + data.SessionID);
      if (subscriptionRequestGuid.HasValue)
      {
        data.SessionID = subscriptionRequestGuid.Value.ToString();
        manager.QueryBySubscriptionRequestGuid(data);
        return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
      }

      Guid? identificationSession = db.LoadGuid("SELECT IdentificationSessionGuid FROM MobilePaywall.core.LookupSession WHERE IdentificationSessionGuid IS NOT NULL AND UserSessionID=" + data.SessionID);
      if(identificationSession.HasValue)
      {
        data.SessionID = identificationSession.Value.ToString();
        manager.QueryByIdentificationSessionGuid(data);
        return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
      }
      
      Guid? referenceGuid = db.LoadGuid("SELECT UserSessionGuid FROM MobilePaywall.core.UserSession WHERE UserSessionID=" + data.SessionID);
      if(referenceGuid.HasValue)
      {
        data.SessionID = referenceGuid.ToString();
        manager.QueryByReferenceGuid(data);
        return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
      }

      return this.Content("Could not find referenceID");
    }

    public ActionResult GetCashflowNewLog(WebLogTableData data)
    {
      MobilePaywallDirect db = MobilePaywallDirect.Instance;
      CashflowLogManager manager = new CashflowLogManager();

      manager.QueryByReferenceIntID(data);
      return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
    }

    // SUMMARY: Report -> Get callback logs
    public ActionResult GetCallbackLog(string SessionID)
    {
      CallbackLogManager manager = new CallbackLogManager();
      return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Query(SessionID) });
    }

    // SUMMARY: Report -> Get UserHttpRequest for specific click
    public ActionResult GetUserHttpReqeusts(WebLogTableData data)
    {
      UserRequestManager manager = new UserRequestManager();
      manager.Query(data);
      return View("~/Views/Report/_LogTable.cshtml", new LogModel { Informations = manager.ConvertToLogModelInfomations(), InformationsLabel = "req" });
    }

    // SUMMARY: Conntect to mysql and get OL data for specific click
    public ActionResult GetOverlayLogs(WebLogTableData data)
    {
      string pxid = MobilePaywallDirect.Instance.LoadString("SELECT Pxid FROM MobilePaywall.core.OLCache WHERE UserSessionID=" + data.SessionID);
      if (string.IsNullOrEmpty(pxid))
        return this.Content("");

      OverlayLogManager manager = new OverlayLogManager();
      data.SessionID = pxid;
      manager.Query(data);
      return View("~/Views/Report/_LogBlock.cshtml", new LogModel() { Tables = manager.Result });
    }

  }


}