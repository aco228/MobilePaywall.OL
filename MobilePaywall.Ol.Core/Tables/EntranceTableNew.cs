using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{
  public class EntranceTableNew
  {
    private DataRow _row = null;

    public int? OLCacheID { get { return this.GetIntValue(Columns.OLCacheID); } }
    public int? UserSessionID { get { return this.GetIntValue(Columns.UserSessionID); } }
    public int? ServiceID { get { return this.GetIntValue(Columns.ServiceID); } }
    public string Pxid { get { return this.GetValue(Columns.Pxid); } }
    public string IPAddress { get { return this.GetValue(Columns.IPAddress); } }
    public string ServiceName { get { return this.GetValue(Columns.ServiceName); } }
    public string ServiceCountry { get { return this.GetValue(Columns.ServiceCountry); } }
    public string CountryName { get { return this.GetValue(Columns.CountryName); } }
    public string CountryCode { get { return this.GetValue(Columns.CountryCode); } }
    public int? MobileOperatorID { get { return this.GetIntValue(Columns.MobileOperatorID); } }
    public string MobileOperatorIDString { get { return this.GetValue(Columns.MobileOperatorID); } }
    public string MobileOperator { get { return this.GetValue(Columns.MobileOperatorName); } }
    public string IdentificationSessionGuid { get { return this.GetValue(Columns.IdentificationSessionGuid); } }
    public string Msisdn { get { return this.GetValue(Columns.Msisdn); } }
    public int? PaymentRequestID { get { return this.GetIntValue(Columns.PaymentRequestID); } }
    public string PaymentRequestIDString { get { return this.GetValue(Columns.PaymentRequestID); } }
    public int? PaymentID { get { return this.GetIntValue(Columns.PaymentID); } }
    public string PaymentIDString { get { return this.GetValue(Columns.PaymentID); } }
    public DateTime? PaymentCreated { get { return this.GetDateValue(Columns.PaymentCreated); } }
    public string PaymentCreatedString { get { return this.GetValue(Columns.PaymentCreated); } }
    public int? PaymentContentAccessPolicyID { get { return this.GetIntValue(Columns.PaymentContentAccessPolicyID); } }
    public string PaymentContentAccessPolicyIDString { get { return this.GetValue(Columns.PaymentContentAccessPolicyID); } }
    public int? TransactionID { get { return this.GetIntValue(Columns.TransactionID); } }
    public string TransactionIDString { get { return this.GetValue(Columns.TransactionID); } }
    public DateTime? TransactionCreated { get { return this.GetDateValue(Columns.TransactionCreated); } }
    public string TransactionCreatedString { get { return this.GetValue(Columns.TransactionCreated); } }
    public string IsSubseguent { get { return this.GetValue(Columns.IsSubseguent); } }
    public DateTime? SessionCreated { get { return this.GetDateValue(Columns.SessionCreated); } }
    
    public string PaymentRequestStatus
    {
      get
      {
        string PaymentRequestStatusID = this.GetValue(Columns.PaymentRequestStatusID);
        switch (PaymentRequestStatusID)
        {
          case "1": return "Initialized";
          case "2": return "Pending";
          case "3": return "Complete";
          case "4": return "Failure";
          case "5": return "PaymentExists";
          default: return "";
        }
      }
    }

    public string PaymentStatus
    {
      get
      {
        string PaymentStatusID = this.GetValue(Columns.PaymentStatusID);
        switch(PaymentStatusID)
        {
          case "1": return "Initialized";
          case "2": return "Pending";
          case "3": return "Successful";
          case "4": return "Failed";
          case "5": return "Cancelled";
          default: return "";
        }
      }
    }

    public string GetDate(DateTime? data)
    {
      if (!data.HasValue)
        return string.Empty;
      return data.Value.ToString("MM/dd/yy H:mm:ss");
    }


    
    public EntranceTableNew(DataRow row)
    {
      this._row = row;
    }

    public string GetValue(Columns type)
    {
      return this._row[(int)type].ToString();
    }

    public int? GetIntValue(Columns type)
    {
      int result = -1;
      if(Int32.TryParse(this._row[(int)type].ToString(), out result))
        return result;
      return null;
    }

    public DateTime? GetDateValue(Columns type)
    {
      DateTime result;
      if (DateTime.TryParse(this._row[(int)type].ToString(), out result))
        return result;
      return null;
    }

    public enum Columns
    {
      OLCacheID,
      UserSessionID,
      ServiceID,
      Pxid,
      IPAddress,
      ServiceName,
      ServiceCountry,
      CountryName,
      CountryCode,
      MobileOperatorID,
      MobileOperatorName,
      IdentificationSessionGuid,
      Msisdn,
      PaymentRequestID,
      PaymentRequestStatusID,
      PaymentID,
      PaymentCreated,
      PaymentStatusID,
      PaymentContentAccessPolicyID,
      TransactionID,
      TransactionCreated,
      IsSubseguent,
      SessionCreated
    }

  }
}
