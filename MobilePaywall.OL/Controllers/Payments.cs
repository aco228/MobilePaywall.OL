using MobilePaywall.Data;
using MobilePaywall.Direct;
using MobilePaywall.Ol.Core;
using MobilePaywall.Ol.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class PaymentsController : Controller
  {

    public ActionResult CloseAccessPolicy(string uid)
    {
      //Client client = ClientHelper.GetClient(Request);
      //if (client == null)
      //  return this.Json(new { status = false, message = "Login error" });

      int _uid = -1;
      if (!Int32.TryParse(uid, out _uid))
        return this.Json(new { status=false, message="Parse error" }, JsonRequestBehavior.AllowGet);

      MobilePaywallDirect db = MobilePaywallDirect.Instance;
      int? paymentID = db.LoadInt("SELECT PaymentID FROM MobilePaywall.core.OLCache WHERE UserSessionID=" + uid);
      if (!paymentID.HasValue)
        return this.Json(new { status = false, message = "Payment is missing from cache" });

      DirectContainer pcapm = db.LoadContainer("SELECT PaymentContentAccessPolicyMapID FROM MobilePaywall.core.PaymentContentAccessPolicyMap WHERE PaymentID=" + paymentID.Value + " AND IsValid=1 AND ValidTo>getdate();");
      if (!pcapm.HasValue)
        return this.Json(new { status = false, message = "There are no active Access policies" });

      IPaymentContentAccessPolicyMapManager pcapManager = PaymentContentAccessPolicyMap.CreateManager();
      foreach(DirectContainerRow row in pcapm.Rows)
      {
        int? pcapmID =  row.GetInt("PaymentContentAccessPolicyMapID");
        if(!pcapmID.HasValue)
          continue;

        PaymentContentAccessPolicyMap map = pcapManager.Load(pcapmID.Value);
        if (map == null)
          continue;
      }

      return null;


    }

  }
}