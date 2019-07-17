using MobilePaywall.Ol.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MobilePaywall.Ol.Core.Models
{
  public class AndroidInputModel
  {
    private string _top = string.Empty;
    private string _from = string.Empty;
    private string _to = string.Empty;
    private string _country = string.Empty;
    private string _service = string.Empty;
    private string _mobileOperator = string.Empty;
    private string _paymentType = string.Empty;
    private string _paymentRequestType = string.Empty;
    private string _transactionType = string.Empty;

    public string Top { get { return this._top; } }
    public string From { get { return this._from; } }
    public string To { get { return this._to; } }
    public string Country { get { return this._country; } }
    public string Service { get { return this._service; } }
    public string MobileOperator { get { return this._mobileOperator; } }
    public string PaymentType { get { return this._paymentType; } }
    public string PaymentRequestType { get { return this._paymentRequestType; } }
    public string TransactionType { get { return this._transactionType; } }

    public AndroidInputModel(HttpRequestBase request)
    {
      this._top = request["rTop"] != null ? request["rTop"].ToString() : string.Empty;
      this._from = request["rFrom"] != null ? request["rFrom"].ToString() : string.Empty;
      this._to = request["rTo"] != null ? request["rTo"].ToString() : string.Empty;
      this._country = request["rCountry"] != null ? request["rCountry"].ToString() : string.Empty;
      this._service = request["rService"] != null ? request["rService"].ToString() : string.Empty;
      this._mobileOperator = request["rMobileOperator"] != null ? request["rMobileOperator"].ToString() : string.Empty;
      this._paymentRequestType = request["rPaymentRequestType"] != null ? request["rPaymentRequestType"].ToString() : string.Empty;
      this._transactionType = request["rTransactionType"] != null ? request["rTransactionType"].ToString() : string.Empty;

      if (string.IsNullOrEmpty(this._mobileOperator))
        this._mobileOperator = "-1";

      this._transactionType = this.ConvertTypes(this._transactionType);
      this._paymentRequestType = this.ConvertTypes(this._paymentRequestType);
      this._paymentType = this.ConvertPaymentType();
    }

    public string ConvertTypes(string original)
    {
      if (original.ToLower().Equals("default"))
        return "-1";
      else if (original.ToLower().Equals("null"))
        return "0";
      else if (original.ToLower().Equals("not null"))
        return "1";
      return " -1;";
    }

    public string ConvertPaymentType()
    {
      switch(this._paymentType.ToLower())
      {
        case "success": return "3";
        case "failed": return "4";
        case "canceled": return "5";
        default: return "-1";
      }
    }

    public EntranceTableData ToEntranceTableData()
    {
      return new EntranceTableData()
      {
        Limit = this._top,
        From = this._from,
        To = this._to,
        Country = this._country,
        MobileOperator = this._mobileOperator,
        Service = this._service,
        PaymentRequest = this._paymentRequestType,
        PaymentStatus = this._paymentType,
        Transaction = this._transactionType
      };
    }

  }
}
