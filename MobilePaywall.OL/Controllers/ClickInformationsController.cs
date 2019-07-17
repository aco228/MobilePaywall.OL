using MobilePaywall.Direct;
using MobilePaywall.Ol.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class ClickInformationsController : Controller
  {

    public ActionResult UserSession()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT  	
 	        us.UserSessionID,
 	        us.UserSessionGuid,
 	        olCache.OLCacheID,
 	        ustype.Name AS 'UserSessionType',
 	        ustype.TypeName AS 'UserSessionTypeName',
 	        country.Globalname AS 'Country',
 	        mo.MobileOperatorID,
 	        mo.ExternalMobileOperatorID,
 	        mo.Name AS 'MobileOperator',
 	        us.FingerprintID,
 	        us.HasVerifiedAge,
 	        us.IsWelcomeShown,
 	        us.IPAddress,
 	        us.UserAgent,
 	        us.EntranceUrl, 
 	        us.Referrer,
 	        us.ValidUntil AS 'UserSessionValidUntil',
 	        us.Created AS 'UserSessionCreated'
         FROM MobilePaywall.core.UserSession AS us  
         LEFT OUTER JOIN MobilePaywall.core.UserSessionType AS ustype ON us.UserSessionTypeID=ustype.UserSessionTypeID 
         LEFT OUTER JOIN MobilePaywall.core.OLCache AS olCache ON olCache.UserSessionID=us.UserSessionID
         LEFT OUTER JOIN MobilePaywall.core.Country AS country ON us.CountryID=country.CountryID
         LEFT OUTER JOIN MobilePaywall.core.MobileOperator AS mo ON us.MobileOperatorID=mo.MobileOperatorID
         WHERE us.UserSessionID=" + uid + "; ");
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Customer()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@" SELECT 
 	        c.CustomerID,
 	        c.CustomerGuid,
 	        c.ExternalCustomerGuid,
 	        cs.Name AS 'CustomerStatus',
 	        mo.MobileOperatorID,
 	        mo.ExternalMobileOperatorID,
 	        mo.Name AS 'MobileOperator',
 	        c.Msisdn,
 	        c.EncryptedMsisdn,
 	        c.Username,
 	        c.Updated,
 	        c.Created
        FROM MobilePaywall.core.Customer AS c
        LEFT OUTER JOIN MobilePaywall.core.CustomerStatus AS cs ON c.CustomerStatusID=cs.CustomerStatusID
        LEFT OUTER JOIN MobilePaywall.core.MobileOperator AS mo ON c.MobileOperatorID=mo.MobileOperatorID
        LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.CustomerID=c.CustomerID
        WHERE us.UserSessionID=" + uid + "; ");
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult LookupSession()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@" SELECT 
	      ls.LookupSessionID,
	      ls.LookupSessionGuid,
	      lss.Name AS 'Status',
	      ls.IdentificationSessionGuid,
	      ls.LookupUrl,
	      lsr.LookupSessionResultID,
	      lsr.IsSuccessful,
	      lsr.IsFinalResult,
	      lsr.IdentificationResultGuid,
	      lsr.MobileOperatorID,
	      lsr.Msisdn,
	      lsr.EncryptedMsisdn,
	      lsr.Created AS 'ResultCreated',
	      ls.Created AS 'LookupSessionCreated'
      FROM MobilePaywall.core.LookupSession AS ls
      LEFT OUTER JOIN MobilePaywall.core.LookupSessionStatus AS lss ON ls.LookupSessionStatusID=lss.LookupSessionStatusID
      LEFT OUTER JOIN MobilePaywall.core.LookupSessionResult AS lsr ON lsr.LookupSessionID=ls.LookupSessionID
      WHERE ls.UserSessionID=" + uid +
      "ORDER BY LookupSessionID DESC");
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult PaymentRequest()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT
	      pr.PaymentRequestID,
	      pr.PaymentRequestGuid,
	      pr.ExternalPaymentRequestGuid,
	      pt.Name AS 'PaymentType',
	      prs.Name AS 'PaymentRequestStatus',
	      bmet.Name AS 'BusinessModelType',
	      pr.PaymentRedirectUrl,
	      pr.Updated,
	      pr.Created
      FROM MobilePaywall.core.PaymentRequest AS pr
      LEFT OUTER JOIN MobilePaywall.core.PaymentType as pt ON pr.PaymentTypeID=pt.PaymentTypeID
      LEFT OUTER JOIN MobilePaywall.core.PaymentRequestStatus AS prs ON pr.PaymentRequestStatusID=prs.PaymentRequestStatusID
      LEFT OUTER JOIN MobilePaywall.core.BusinessModelEntry AS bme ON pr.BusinessModelEntryID=bme.BusinessModelEntryID
      LEFT OUTER JOIN MobilePaywall.core.BusinessModelType AS bmet ON bme.BusinessModelTypeID=bmet.BusinessModelTypeID
      WHERE pr.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Payment()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT
	      p.PaymentID,
	      p.PaymentGuid,
	      p.ExternalPaymentGuid,
	      pt.Name AS 'PaymentType',
	      ps.Name AS 'PaymentStatus',
	      p.Updated,
	      p.Created
      FROM MobilePaywall.core.Payment AS p
      LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON p.PaymentRequestID=pr.PaymentRequestID
      LEFT OUTER JOIN MobilePaywall.core.PaymentType AS pt ON p.PaymentTypeID=pt.PaymentTypeID
      LEFT OUTER JOIN MobilePaywall.core.PaymentStatus AS ps ON p.PaymentStatusID=ps.PaymentStatusID
      WHERE pr.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult PaymentAccessPolicy()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	      pcapm.PaymentContentAccessPolicyMapID,
	      pcapm.ContentID,
	      pcapm.TransactionID,
	      pcapm.IsValid,
	      pcapm.IsTemporary,
	      pcapm.ValidTo,
	      pcapm.Updated,
	      pcapm.Created
      FROM MobilePaywall.core.PaymentContentAccessPolicyMap AS pcapm
      LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON pcapm.PaymentID=p.PaymentID
      LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON pr.PaymentRequestID=p.PaymentRequestID
      WHERE pr.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Transaction()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	        t.TransactionID,
	        t.TransactionGuid,
	        t.ExternalTransactionGuid,
	        t.ExternalTransactionGroupGuid,
	        ts.Name AS 'TransactionStatus',
	        tt.Name AS 'TransactionType',
	        t.Updated,
	        t.Created
        FROM MobilePaywall.core.[Transaction] AS t 
        LEFT OUTER JOIN MobilePaywall.core.TransactionStatus AS ts ON t.TransactionStatusID=ts.TransactionStatusID
        LEFT OUTER JOIN MobilePaywall.core.TransactionType AS tt ON t.TransactionTypeID=tt.TransactionTypeID
        LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID
        LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON pr.PaymentRequestID=p.PaymentRequestID
        WHERE pr.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Service()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	      s.ServiceID,
	      s.Name,
	      s.Description,
	      ss.Name AS 'Status',
	      st.Name AS 'Type',
	      l.GlobalName AS 'FallbackLanguage',
	      c.GlobalName AS 'FallbackCountry',
	      ar.Name AS 'FallbackAgeRating',
	      bm.Name AS 'BusinessModel',
	      t.Name AS 'Template',
	      s.Updated,
	      s.Created
      FROM MobilePaywall.core.Service AS s
      LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.ServiceID=s.ServiceID
      LEFT OUTER JOIN MobilePaywall.core.ServiceStatus AS ss ON s.ServiceStatusID=ss.ServiceStatusID
      LEFT OUTER JOIN MobilePaywall.core.ServiceType AS st ON s.ServiceTypeID=st.ServiceTypeID
      LEFT OUTER JOIN MobilePaywall.core.Language AS l ON s.FallbackLanguageID=l.LanguageID
      LEFT OUTER JOIN MobilePaywall.core.Country AS c ON s.FallbackCountryID=c.CountryID
      LEFT OUTER JOIN MobilePaywall.core.Template AS t ON s.TemplateID=t.TemplateID
      LEFT OUTER JOIN MobilePaywall.core.AgeRating AS ar ON s.FallbackAgeRatingID=ar.AgeRatingID
      LEFT OUTER JOIN MobilePaywall.core.BusinessModel AS bm ON s.BusinessModelID=bm.BusinessModelID
      WHERE us.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Application()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	      a.ApplicationID,
	      a.Name,
	      a.ApplicationName,
	      i.Name AS 'Instance',
	      at.Name AS 'ApplicationType',
	      rt.Name AS 'RuntimeType',
	      rt.TypeName AS 'RuntimeType',
	      a.Updated,
	      a.Created
      FROM MobilePaywall.core.Application AS a 
      LEFT OUTER JOIN MobilePaywall.core.Service AS s ON s.ApplicationID=a.ApplicationID
      LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.ServiceID=s.ServiceID
      LEFT OUTER JOIN MobilePaywall.core.Instance AS i ON a.InstanceID=i.InstanceID
      LEFT OUTER JOIN MobilePaywall.core.RuntimeType AS rt ON a.RuntimeTypeID=rt.RuntimeTypeID
      LEFT OUTER JOIN MobilePaywall.core.ApplicationType AS at ON a.ApplicationTypeID=at.ApplicationTypeID
      WHERE us.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Product()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	        p.ProductID,
	        p.ProductGuid,
	        p.ExternalProductGuid,
	        i.Name,
	        p.Name,
	        p.Description,
	        p.Updated,
	        p.Created
        FROM MobilePaywall.core.Product AS p
        LEFT OUTER JOIN MobilePaywall.core.Service AS s ON s.ProductID=p.ProductID
        LEFT OUTER JOIN MobilePaywall.core.Instance AS i ON p.InstanceID=i.InstanceID
        LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.ServiceID=s.ServiceID
        WHERE us.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Merchant()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	      m.MerchantID,
	      m.Name,
	      m.Address,
	      m.Phone,
	      m.Email,
	      m.RegistrationNo,
	      m.VatNo,
	      t.Name AS 'Template',
	      m.Updated,
	      m.Created
      FROM MobilePaywall.core.Merchant AS m
      LEFT OUTER JOIN MobilePaywall.core.Service AS s ON s.MerchantID=m.MerchantID
      LEFT OUTER JOIN MobilePaywall.core.Template AS t ON m.TemplateID=t.TemplateID
      LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.ServiceID=s.ServiceID
      WHERE us.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult ContentAccessPolicy()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	      c.ContentAccessPolicyID,
	      bm.Name AS 'BusinessModel',
	      tsam.Name AS 'TemporarySetupAccessModelID',
	      c.TemporaryAccessInSeconds,
	      c.NumberOfItems,
	      c.NumberOfItemDownloads,
	      c.NumberOfTotalDownloads,
	      c.IntervalInSeconds,
	      c.Updated,
	      c.Created
      FROM MobilePaywall.core.ContentAccessPolicy AS c
      LEFT OUTER JOIN MobilePaywall.core.Service AS s ON c.ServiceID=s.ServiceID
      LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.ServiceID=s.ServiceID
      LEFT OUTER JOIN MobilePaywall.core.BusinessModel AS bm ON c.BusinessModelID=bm.BusinessModelID
      LEFT OUTER JOIN MobilePaywall.core.TemporarySetupAccessMode AS tsam ON c.TemporarySetupAccessModeID=tsam.TemporarySetupAccessModeID
      WHERE us.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult PaymentProvider()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      #region load method
      DirectContainer container = new MobilePaywallDirect().LoadContainer(@"SELECT 
	      pr.PaymentProviderID,
	      pr.ExternalPaymentProviderGuid,
	      pr.Name,
	      pr.Updated,
	      pr.Created
      FROM MobilePaywall.core.PaymentProvider AS pr
      LEFT OUTER JOIN MobilePaywall.core.PaymentConfiguration AS pc ON pc.PaymentProviderID=pr.PaymentProviderID
      LEFT OUTER JOIN MobilePaywall.core.ServiceOffer AS so ON so.PaymentConfigurationID=pc.PaymentConfigurationID
      LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON so.ServiceID=us.ServiceID
      WHERE us.UserSessionID=" + uid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Kiwiclick()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      string pxid = MobilePaywallDirect.Instance.LoadString("SELECT Pxid FROM MobilePaywall.core.OLCache WHERE UserSessionID=" + uid);
      if (string.IsNullOrEmpty(pxid))
        return this.Content("");

      #region load method
      DirectContainer container = new KiwiclicksDirect().LoadContainer(@"SELECT 
	        c.click_id AS 'ID',
            from_unixtime(c.click_time, '%Y-%m-%d %h:%i:%s') AS 'Date',
            from_unixtime(c.cvr_time, '%Y-%m-%d %h:%i:%s') AS 'ConversionTime',
            c.click_ip  AS 'IPAddress',
            c.click_useragent AS 'UserAgent',
            camp.camp_id AS 'CampaignID',
            camp.camp_name AS 'CampaignName',
            g.group_id AS 'GroupID',
            g.group_name AS 'GroupName',
            offer.offer_id AS 'OfferID',
            offer.offer_name AS 'OfferName',
            c.click_os AS 'OS',
            c.click_brand AS 'Brand',
            c.name AS 'Name',
            c.click_model AS 'Model',
            c.nameModel AS 'NameModel',
            c.deviceType AS 'DeviceType',
            c.displaySize AS 'DisplaySize',
	        c.resolution AS 'Resolution'
        FROM mtkiwiclick.mt_click AS c 
        LEFT OUTER JOIN mtkiwiclick.mt_campaigns AS camp ON c.camp_id=camp.camp_id
        LEFT OUTER JOIN mtkiwiclick.mt_groups AS g ON camp.group_id=g.group_id
        LEFT OUTER JOIN mtkiwiclick.mt_offers as offer ON c.offer_id=offer.offer_id
        WHERE c.click_subid=" + pxid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }

    public ActionResult Bananaclicks()
    {
      int _uid = -1;
      string uid = Request["uid"] != null ? Request["uid"].ToString() : string.Empty;
      if (!Int32.TryParse(uid, out _uid))
        return this.Content("Could not parse UID");

      string pxid = MobilePaywallDirect.Instance.LoadString("SELECT Pxid FROM MobilePaywall.core.OLCache WHERE UserSessionID=" + uid);
      if (string.IsNullOrEmpty(pxid))
        return this.Content("");

      #region load method
      DirectContainer container = new BananaclicksDirect().LoadContainer(@"SELECT 
	        c.click_id AS 'ID',
            from_unixtime(c.click_time, '%Y-%m-%d %h:%i:%s') AS 'Date',
            from_unixtime(c.cvr_time, '%Y-%m-%d %h:%i:%s') AS 'ConversionTime',
            c.click_ip  AS 'IPAddress',
            c.click_useragent AS 'UserAgent',
            camp.camp_id AS 'CampaignID',
            camp.camp_name AS 'CampaignName',
            g.group_id AS 'GroupID',
            g.group_name AS 'GroupName',
            offer.offer_id AS 'OfferID',
            offer.offer_name AS 'OfferName',
            c.click_os AS 'OS',
            c.click_brand AS 'Brand',
            c.name AS 'Name',
            c.click_model AS 'Model',
            c.nameModel AS 'NameModel',
            c.deviceType AS 'DeviceType',
            c.displaySize AS 'DisplaySize',
	        c.resolution AS 'Resolution'
        FROM mtkiwiclick.mt_click AS c 
        LEFT OUTER JOIN mtkiwiclick.mt_campaigns AS camp ON c.camp_id=camp.camp_id
        LEFT OUTER JOIN mtkiwiclick.mt_groups AS g ON camp.group_id=g.group_id
        LEFT OUTER JOIN mtkiwiclick.mt_offers as offer ON c.offer_id=offer.offer_id
        WHERE c.click_subid=" + pxid);
      #endregion

      InformationsBaseTableModel model = new InformationsBaseTableModel(container);
      return PartialView("~/Views/Report/_InformationsBaseTable.cshtml", model);
    }


    public ActionResult ClickinformationsAction_CancelPayment(string uid)
    {
      if (string.IsNullOrEmpty(uid))
        return this.Json(new { status=false, message="There is no uid param" });

      MobilePaywallDirect db = MobilePaywallDirect.Instance;

      DirectContainer container = db.LoadContainer(string.Format(@"
        SELECT ol.PaymentID, p.PaymentStatusID FROM MobilePaywall.core.OLCache AS ol
        LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON ol.PaymentID=p.PaymentID
        WHERE UserSessionID={0}", uid));

      if(!container.HasValue)
        return this.Json(new { status = false, message = "There is no entry for this UserSession" });

      int? paymentID = container.GetInt("PaymentID");
      int? paymentStatus = container.GetInt("PaymentStatusID");

      if (!paymentID.HasValue)
        return this.Json(new { status = false, message = "This UserSession has no payment" });
      if (!paymentStatus.HasValue)
        return this.Json(new { status = false, message = "InternalError. There is no status for payment on this US" });

      if (paymentStatus.Value == 1)
        return this.Json(new { status = false, message = "Payment status is initialized. You cannot cancel this status" });
      if (paymentStatus.Value == 2)
        return this.Json(new { status = false, message = "Payment status is pending. You cannot cancel this status" });
      if (paymentStatus.Value == 4)
        return this.Json(new { status = false, message = "Payment status is failed. You cannot cancel this payment" });
      if (paymentStatus.Value == 5)
        return this.Json(new { status = false, message = "This payment is allready canceled" });

      db.Execute("UPDATE MobilePaywall.core.Payment SET PaymentStatusID=5 WHERE PaymentID=" + paymentID.Value);
      return this.Json(new { status = true, message = "Payment is cancelled" });
    }

    public ActionResult ClickinformationsAction_CancelAccessPolicy(string uid)
    {
      if (string.IsNullOrEmpty(uid))
        return this.Json(new { status = false, message = "There is no uid param" });

      MobilePaywallDirect db = MobilePaywallDirect.Instance;
      
      int? paymentID = db.LoadInt(string.Format(@"
        SELECT ol.PaymentID FROM MobilePaywall.core.OLCache AS ol
        WHERE UserSessionID={0}", uid));

      if (!paymentID.HasValue)
        return this.Json(new { status=false, message= "This UserSession has no Payment" });
      
      db.Execute("UPDATE MobilePaywall.core.PaymentContentAccessPolicyMap SET IsValid=0 WHERE PaymentID=" + paymentID.Value);
      return this.Json(new { status = true, message = "AccessPolicy is closed" });
    }

  }
}