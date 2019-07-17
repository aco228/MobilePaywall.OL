using MobilePaywall.Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class OLCacheModel
  {
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public string SessionCreated { get; set; }
    public string Pxid { get; set; }
    public string ServiceName { get; set; }
    public string MobileOperatorName { get; set; }
    public string Msisdn { get; set; }
    public string IPAddress { get; set; }
    public string PaymentRequestStatus { get; set; }
    public string PaymentStatus { get; set; }
    public string PaymentCreated { get; set; }
    public string AccessPolicy { get; set; }
    public string TransactionID { get; set; }
    public string TransactionCreated { get; set; }

    public string GetDate(string date)
    {
      DateTime data;

      if (!DateTime.TryParse(date, out data))
        return string.Empty;

      return data.ToString("MM/dd/yy H:mm:ss");
    }
    
  }
}
