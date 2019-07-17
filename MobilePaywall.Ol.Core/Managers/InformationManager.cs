using MobilePaywall.Ol.Core.Data;
using MobilePaywall.Ol.Core.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Managers
{
  public class InformationManager : MobilePaywallDatabase
  {
    private List<List<string[]>> _result = null;
    #region Columns
    private string[] _columns = new string[] 
    {
      /*us.UserSessionID*/ "UserSessionID" ,
      /*us.UserSessionGuid*/ "UserSessionGuid" ,
      /*ustype.Name*/ "UserSessionType" ,
      /*s.Name*/ "Service" ,
      /*us.TrackingID*/ "TrackingID" ,
      /*us.FingerprintID*/ "FingerprintID" ,
      /*us.HasVerifiedAge*/ "HasVerifiedAge" ,
      /*us.IsWelcomeShown*/ "IsWelcomeShown" ,
      /*us.IPAddress*/ "IPAddress" ,
      /*us.UserAgent*/ "UserAgent" ,
      /*us.EntranceUrl*/ "EntranceUrl" ,
      /*us.Referrer*/ "Referrer" ,
      /*us.ValidUntil*/ "ValidUntil" ,
      /*us.Created*/ "UserSessionCreated" ,
      /*mo.MobileOperatorID*/ "MobileOperatorID" ,
      /*mo.Name*/ "MobileOperator" ,
      /*mo.ExternalMobileOperatorID*/ "ExternalMobileOperatorID" ,
      /*county.GlobalName*/ "Country" ,
      /*cst.CustomerID*/ "CustomerID" ,
      /*cst.CustomerGuid*/ "CustomerGuid" ,
      /*cst.ExternalCustomerGuid*/ "ExternalCustomerGuid" ,
      /*cst.Msisdn*/ "Msisdn" ,
      /*cst.EncryptedMsisdn*/ "EncryptedMsisdn" ,
      /*cst.Username*/ "Username" ,
      /*cstStatus.Name*/ "CustomerStatus" ,
      /*cst.Created*/ "CustomerCreated" ,
      /*ls.LookupSessionID*/ "LookupSessionID" ,
      /*ls.LookupSessionGuid*/ "LookupSessionGuid" ,
      /*lsStatus.Name*/ "LookupSessionStatus" ,
      /*ls.IdentificationSessionGuid*/ "IdentificationSessionGuid" ,
      /*ls.LookupUrl*/ "LookupUrl" ,
      /*lsr.LookupSessionResultID*/ "LookupSessionResultID" ,
      /*lsr.IsSuccessful*/ "IsSuccessful" ,
      /*lsr.IsFinalResult*/ "IsFinalResult" ,
      /*pr.PaymentRequestID*/ "PaymentRequestID" ,
      /*pr.PaymentRequestGuid*/ "PaymentRequestGuid" ,
      /*pType.Name*/ "PaymentType" ,
      /*prStatus.Name*/ "PaymentStatus" ,
      /*pr.ExternalPaymentRequestGuid*/ "ExternalPaymentRequestGuid" ,
      /*pr.PaymentRedirectUrl*/ "PaymentRedirectUrl" ,
      /*pr.Created*/ "PaymentRequsetCreated" ,
      /*p.PaymentID*/ "PaymentID" ,
      /*p.PaymentGuid*/ "PaymentGuid" ,
      /*p.ExternalPaymentGuid*/ "ExternalPaymentGuid" ,
      /*pStatus.Name*/ "PaymentStatus" ,
      /*p.Created*/ "PaymentCreated" ,
      /*pcapm.PaymentContentAccessPolicyMapID*/ "PaymentContentAccessPolicyMapID" ,
      /*pcapm.ContentID*/ "ContentID" ,
      /*pcapm.ContentAccessPolicyID*/ "ContentAccessPolicyID" ,
      /*pcapm.IsValid*/ "IsValid" ,
      /*pcapm.IsTemporary*/ "IsTemporary" ,
      /*pcapm.ValidFrom*/ "ValidFrom" ,
      /*pcapm.ValidTo*/ "ValidTo" ,
      /*t.TransactionID*/ "TransactionID" ,
      /*t.TransactionGuid*/ "TransactionGuid" ,
      /*t.ExternalTransactionGuid*/ "ExternalTransactionGuid" ,
      /*tStatus.Name*/ "TransactionStatus" ,
      /*tType.Name*/ "TransactionType" ,
      /*t.Created*/ "TransactionCreated" 
    };
    #endregion

    public List<List<string[]>> Result { get { return this._result; } set { this._result = value; } }

    public InformationManager() : base() { }

    public List<List<string[]>> Query(WebLogTableData data)
    {
      string command = "";
      #region # command #
      command = " SELECT " +
              " 	us.UserSessionID, " +
              " 	us.UserSessionGuid, " +
              " 	ustype.Name, " +
              " 	s.Name, " +
              " 	us.TrackingID, " +
              " 	us.FingerprintID, " +
              " 	us.HasVerifiedAge, " +
              " 	us.IsWelcomeShown, " +
              " 	us.IPAddress, " +
              " 	us.UserAgent, " +
              " 	us.EntranceUrl, " +
              " 	us.Referrer, " +
              " 	us.ValidUntil, " +
              " 	us.Created, " +
              " 	mo.MobileOperatorID, " +
              " 	mo.Name, " +
              " 	mo.ExternalMobileOperatorID, " +
              " 	county.GlobalName, " +
              " 	cst.CustomerID, " +
              " 	cst.CustomerGuid, " +
              " 	cst.ExternalCustomerGuid, " +
              " 	cst.Msisdn, " +
              " 	cst.EncryptedMsisdn, " +
              " 	cst.Username, " +
              " 	cstStatus.Name, " +
              " 	cst.Created, " +
              " 	ls.LookupSessionID, " +
              " 	ls.LookupSessionGuid, " +
              " 	lsStatus.Name, " +
              " 	ls.IdentificationSessionGuid, " +
              " 	ls.LookupUrl, " +
              " 	lsr.LookupSessionResultID, " +
              " 	lsr.IsSuccessful, " +
              " 	lsr.IsFinalResult, " +
              " 	pr.PaymentRequestID, " +
              " 	pr.PaymentRequestGuid, " +
              " 	pType.Name, " +
              " 	prStatus.Name, " +
              " 	pr.ExternalPaymentRequestGuid, " +
              " 	pr.PaymentRedirectUrl, " +
              " 	pr.Created, " +
              " 	p.PaymentID, " +
              " 	p.PaymentGuid, " +
              " 	p.ExternalPaymentGuid, " +
              " 	pStatus.Name, " +
              " 	p.Created, " +
              " 	pcapm.PaymentContentAccessPolicyMapID, " +
              " 	pcapm.ContentID, " +
              " 	pcapm.ContentAccessPolicyID, " +
              " 	pcapm.IsValid, " +
              " 	pcapm.IsTemporary, " +
              " 	pcapm.ValidFrom, " +
              " 	pcapm.ValidTo, " +
              " 	t.TransactionID, " +
              " 	t.TransactionGuid, " +
              " 	t.ExternalTransactionGuid, " +
              " 	tStatus.Name, " +
              " 	tType.Name, " +
              " 	t.Created " +
              " FROM MobilePaywall.core.UserSession AS us " +
              " LEFT OUTER JOIN MobilePaywall.core.UserSessionType AS ustype ON us.UserSessionTypeID=ustype.UserSessionTypeID " +
              " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
              " LEFT OUTER JOIN MobilePaywall.core.MobileOperator AS mo ON us.MobileOperatorID=mo.MobileOperatorID " +
              " LEFT OUTER JOIN MobilePaywall.core.Country AS county ON us.CountryID=county.CountryID " +
              " LEFT OUTER JOIN MobilePaywall.core.Customer AS cst ON cst.CustomerID=us.CustomerID " +
              " LEFT OUTER JOIN MobilePaywall.core.CustomerStatus AS cstStatus ON cstStatus.CustomerStatusID=cst.CustomerStatusID " +
              " LEFT OUTER JOIN MobilePaywall.core.LookupSession AS ls ON us.UserSessionID=ls.UserSessionID " +
              " LEFT OUTER JOIN MobilePaywall.core.LookupSessionStatus AS lsStatus ON lsStatus.LookupSessionStatusID=ls.LookupSessionStatusID " +
              " LEFT OUTER JOIN MobilePaywall.core.LookupSessionResult AS lsr ON lsr.LookupSessionID=ls.LookupSessionID " +
              " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON us.UserSessionID=pr.UserSessionID " +
              " LEFT OUTER JOIN MobilePaywall.core.PaymentType AS pType ON pType.PaymentTypeID=pr.PaymentTypeID " +
              " LEFT OUTER JOIN MobilePaywall.core.PaymentRequestStatus AS prStatus ON prStatus.PaymentRequestStatusID=pr.PaymentRequestStatusID " +
              " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON p.PaymentRequestID=pr.PaymentRequestID " +
              " LEFT OUTER JOIN MobilePaywall.core.PaymentStatus AS pStatus ON pStatus.PaymentStatusID=p.PaymentStatusID " +
              " LEFT OUTER JOIN MobilePaywall.core.PaymentContentAccessPolicyMap AS pcapm ON pcapm.PaymentID=p.PaymentID " +
              " LEFT OUTER JOIN MobilePaywall.core.[Transaction] AS t ON t.PaymentID=p.PaymentID " +
              " LEFT OUTER JOIN MobilePaywall.core.TransactionStatus AS tStatus ON tStatus.TransactionStatusID = t.TransactionStatusID " +
              " LEFT OUTER JOIN MobilePaywall.core.TransactionType AS tType ON tType.TransactionTypeID=t.TransactionTypeID " +
              " WHERE us.UserSessionID='"+ data.SessionID +"' " +
              " AND us.Created >= '"+ data.From +"' AND us.Created <= '"+ data.To +"'; ";
      #endregion

      this._result = new List<List<string[]>>();
      this.SetTimeout(0);
      DataTable table = this.Load(command);
      if (table == null)
        return this._result;


      foreach (DataRow row in table.Rows)
      {
        List<string[]> list = new List<string[]>();
        for (int i = 0; i < this._columns.Length;i++ )
          list.Add(new string[] { this._columns[i], row[i].ToString() });
        this._result.Add(list);
      }
      
      return this._result;
    }

  }
}
