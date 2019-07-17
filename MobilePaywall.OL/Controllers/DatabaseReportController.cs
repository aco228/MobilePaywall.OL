using MobilePaywall.Data;
using MobilePaywall.Direct;
using MobilePaywall.Ol.Core.Data;
using MobilePaywall.Ol.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class DatabaseReportController : Controller
  {

    public ActionResult Index(EntranceTableData data)
    {
      MobilePaywall.Direct.ServerDirect db = new ServerDirect();
      DataTable table = db.Load(@"SELECT DISTINCT 
                                  SJ.Name AS JobName, 
                                  SJ.description AS JobDescription,
                                  SJH.run_date AS LastRunDate, 
                                  CASE SJH.run_status 
                                  WHEN 0 THEN 'Failed'
                                  WHEN 1 THEN 'Successful'
                                  WHEN 3 THEN 'Cancelled'
                                  WHEN 4 THEN 'In Progress'
                                  END AS LastRunStatus
                                  FROM sysjobhistory SJH, sysjobs SJ
                                  WHERE SJH.job_id = SJ.job_id and SJH.run_date =  (SELECT MAX(SJH1.run_date) FROM sysjobhistory SJH1 WHERE SJH.job_id = SJH1.job_id)
                                  AND SJ.name LIKE 'MobilePaywall%'
                                  ORDER BY SJH.run_date desc");

      if (table == null || table.Rows.Count == 0)
        return this.Content("Load error");

      List<DatabaseReportActivityEntry> dataEntries = new List<DatabaseReportActivityEntry>();
      foreach (DataRow row in table.Rows)
        dataEntries.Add(new DatabaseReportActivityEntry(row));

      return View("~/Views/Statistic/DatabaseReport.cshtml", new DatabaseReportModel(dataEntries));
    }

    public ActionResult GetClickInformations(string uid)
    {
      int _uid = -1;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("");

      MobilePaywallDirect db = MobilePaywallDirect.Instance;
      DirectContainer cache = db.LoadContainer(@"SELECT c.ServiceName, c.CountryCode, c.MobileOperatorName, c.SessionCreated, p.PaymentStatusID, p.PaymentID
        FROM MobilePaywall.core.OLCache AS c
        LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON c.PaymentID=p.PaymentID
        WHERE c.UserSessionID=" + uid);

      if (!cache.HasValue)
        return this.Content("");
      
      ClickInformationModel model = new ClickInformationModel();
      model.UserSessionID = uid;
      model.Logo = string.Format("http://{0}/logo", cache.GetString("ServiceName"));
      model.Country = string.Format("../images/_flagsx/{0}.png", cache.GetString("CountryCode"));

      return PartialView("~/Views/Report/_ClickInformations.cshtml", model);
    }

    public ActionResult GetOLInfo(string uid)
    {
      int usid = -1;
      if (!Int32.TryParse(uid, out usid))
        return this.Content("Parse error");

      UserSession us = UserSession.CreateManager().Load(usid);
      if (us == null)
        return this.Content("User session error");

      DirectContainer olContainer = (new MobilePaywallDirect()).LoadContainer(@"SELECT c.SessionCreated, c.Msisdn, c.CountryCode, c.Pxid, c.ServiceName, c.MobileOperatorName, c.IPAddress, prs.Name AS 'PaymentRequestStatus', 
              ps.Name AS 'PaymentStatus', p.Created AS 'PaymentCreated', c.PaymentContentAccessPolicyID AS 'AccessPolicy', c.TransactionID, c.Created AS 'TransactionCreated'
         FROM MobilePaywall.core.OLCache AS c
         LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON c.PaymentRequestID=pr.PaymentRequestID
         LEFT OUTER JOIN MobilePaywall.core.PaymentRequestStatus AS prs ON pr.PaymentRequestStatusID=prs.PaymentRequestStatusID
         LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON c.PaymentID=p.PaymentID
         LEFT OUTER JOIN MobilePaywall.core.PaymentStatus AS ps ON p.PaymentStatusID=ps.PaymentStatusID
         WHERE c.UserSessionID=" + uid +" AND IsSubseguent=0 ORDER BY c.OLCacheID DESC");

      if (olContainer == null)
        return this.Content("No Data");

      OLCacheModel model = olContainer.Convert<OLCacheModel>();
      return PartialView("~/Views/Report/_OLBlock.cshtml", model); 
    }

    public ActionResult GetInformations(string uid)
    {
      ViewBag.UserSessionID = uid;

      return PartialView("~/Views/Report/_InformationsBase.cshtml");
    }
    

  }
}