using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{
  public class EntranceTable
  {
    private DataRow _row = null;

    public string UserSessionID { get { return this._row != null ? this._row[(int)EntranceTableColums.UserSessionID].ToString() : string.Empty; } }
    public string CountryCode { get { return this._row != null ? this._row[(int)EntranceTableColums.CountryIsoCode].ToString() : string.Empty; } }
    public string CountryName { get { return this._row != null ? this._row[(int)EntranceTableColums.CountryName].ToString() : string.Empty; } }
    public Guid? UserSessionGuid { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.UserSessionGuid].ToString()) ? Guid.Parse(this._row[(int)EntranceTableColums.UserSessionGuid].ToString()) : (Guid?)null; } }
    public string UserSessionGuidString { get { return this._row != null ? this._row[(int)EntranceTableColums.UserSessionCreated].ToString() : string.Empty; } }
    public DateTime? UserSessionCreated { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.UserSessionCreated].ToString()) ? DateTime.Parse(this._row[(int)EntranceTableColums.UserSessionCreated].ToString()) : (DateTime?)null; ; } }
    public string UserSessionCreatedString { get { return this.UserSessionCreated.HasValue ? this.UserSessionCreated.Value.ToString("yyyy-MM-dd - HH:mm:ss") : string.Empty; } }
    public string ServiceName { get { return this._row != null ? this._row[(int)EntranceTableColums.ServiceName].ToString() : string.Empty; } }
    public string MobileOperator { get { return this._row != null ? this._row[(int)EntranceTableColums.MobileOperator].ToString() : string.Empty; } }
    public Guid? IdentificationSessionGuid { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.IdentificationSessionGuid].ToString()) ? Guid.Parse(this._row[(int)EntranceTableColums.IdentificationSessionGuid].ToString()) : (Guid?)null; } }
    public string IPAddress { get { return this._row != null ? this._row[(int)EntranceTableColums.IPAddress].ToString() : string.Empty; } }
    public string Msisdn { get { return this._row != null ? this._row[(int)EntranceTableColums.Msisdn].ToString() : string.Empty; } }
    public int? PaymentRequestID { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.PaymentRequestID].ToString()) ? Int32.Parse(this._row[(int)EntranceTableColums.PaymentRequestID].ToString()) : (int?)null; } }
    public string PaymentRequestStatus { get { return this._row != null ? this._row[(int)EntranceTableColums.PaymentReqeustStatus].ToString() : string.Empty; } }
    public Guid? ExternalPaymentRequestGuid { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.ExternalPaymentRequestGuid].ToString()) ? Guid.Parse(this._row[(int)EntranceTableColums.ExternalPaymentRequestGuid].ToString()) : (Guid?)null; } }
    public string ExternalPaymentRequestGuidString { get { return this.ExternalPaymentGuid.HasValue ? this.ExternalPaymentGuid.Value.ToString() : string.Empty; } }
    public string PaymentRedirectUrl { get { return this._row != null ? this._row[(int)EntranceTableColums.PaymentRedirectUrl].ToString() : string.Empty; } }
    public Guid? ExternalPaymentGuid { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.ExternalPaymentGuid].ToString()) ? Guid.Parse(this._row[(int)EntranceTableColums.ExternalPaymentGuid].ToString()) : (Guid?)null; } }
    public string ExternalPaymentGuidString { get { return this.ExternalPaymentGuid.HasValue ? this.ExternalPaymentGuid.ToString() : string.Empty; } }
    public string PaymentStatus { get { return this._row != null ? this._row[(int)EntranceTableColums.PaymentStatus].ToString() : string.Empty; } }
    public DateTime? PaymentCreated { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.PaymentCreated].ToString()) ? DateTime.Parse(this._row[(int)EntranceTableColums.PaymentCreated].ToString()) : (DateTime?)null; } }
    public string PaymentCreatedString { get { return this.PaymentCreated.HasValue ? this.PaymentCreated.Value.ToString("yyyy-MM-dd - HH:mm:ss") : string.Empty; } }
    public int? PaymentContentAccessPolicyID { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.PaymentContentAccessPolicyMapID].ToString()) ? Int32.Parse(this._row[(int)EntranceTableColums.PaymentContentAccessPolicyMapID].ToString()) : (int?)null; } }
    public string PaymentContentAccessPolicyIDString { get { return this._row != null ? this._row[(int)EntranceTableColums.PaymentContentAccessPolicyMapID].ToString() : string.Empty; } }
    public int? TransactionID { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.TransactionID].ToString()) ? Int32.Parse(this._row[(int)EntranceTableColums.TransactionID].ToString()) : (int?)null; } }
    public string TransactionIDString { get { return this._row != null ? this._row[(int)EntranceTableColums.TransactionID].ToString() : string.Empty; } }
    public DateTime? TransactionCreated { get { return this._row != null && !string.IsNullOrEmpty(this._row[(int)EntranceTableColums.TransactionCreated].ToString()) ? DateTime.Parse(this._row[(int)EntranceTableColums.TransactionCreated].ToString()) : (DateTime?)null; } }
    public string TransactionCreatedString { get { return this.TransactionCreated.HasValue ? this.TransactionCreated.Value.ToString("yyyy-MM-dd - HH:mm:ss") : string.Empty; } }
    public string EntranceUrl { get { return this._row[(int)EntranceTableColums.EntranceUrl].ToString(); } }

    public EntranceTable(DataRow row)
    {
      this._row = row;
    }

    public string Pxid
    {
      get
      {
        if (string.IsNullOrEmpty(this.EntranceUrl))
          return string.Empty;

        Regex regex = new Regex("(pxid=)([0-9]+)");
        Match match = regex.Match(this.EntranceUrl);
        if (match.Success)
          return match.Groups[2].Value;
        return string.Empty;
      }
    }

    public string CsvRow
    {
      get
      {
        return this.CountryCode + "; " +
               this.CountryName + "; " +
               this.UserSessionGuidString + "; " +
               this.UserSessionCreatedString + "; " +
               this.Pxid + "; " + 
               this.EntranceUrl + "; " +
               this.ServiceName + "; " +
               this.MobileOperator + "; " +
               (this.IdentificationSessionGuid.HasValue ? this.IdentificationSessionGuid.ToString() : "") + "; " +
               this.IPAddress + "; " +
               this.Msisdn + "; " +
               (this.PaymentRequestID.HasValue ? this.PaymentRequestID.ToString() : "") + "; " +
               this.PaymentRequestStatus + "; " +
               this.ExternalPaymentRequestGuidString + "; " +
               this.PaymentRedirectUrl + "; " +
               this.ExternalPaymentGuidString + "; " +
               this.PaymentStatus + "; " +
               this.PaymentCreatedString + "; " +
               this.PaymentContentAccessPolicyIDString + "; " +
               this.TransactionIDString + "; " +
               this.TransactionCreatedString + "; ";
      }
    }

  }

  public enum EntranceTableColums
  {
    UserSessionID,
    CountryIsoCode,
    CountryName,
    UserSessionGuid,
    UserSessionCreated,
    EntranceUrl,
    ServiceName,
    MobileOperator,
    IdentificationSessionGuid,
    IPAddress,
    Msisdn,
    PaymentRequestID,
    PaymentReqeustStatus,
    ExternalPaymentRequestGuid,
    PaymentRedirectUrl,
    ExternalPaymentGuid,
    PaymentStatus,
    PaymentCreated,
    PaymentContentAccessPolicyMapID,
    TransactionID,
    TransactionCreated
  }
}
