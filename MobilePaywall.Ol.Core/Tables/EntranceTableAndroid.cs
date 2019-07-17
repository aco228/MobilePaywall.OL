using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{

  public class EntranceTableAndroid
  {
    private DataRow _row = null;

    public int? UserSessionID { get { return this.GetIntValue(Columns.UserSessionID); } }
    public string ServiceName { get { return this.GetValue(Columns.ServiceName); } }
    public string CountryName { get { return this.GetValue(Columns.CountryName); } }
    public string MobileOperator { get { return this.GetValue(Columns.MobileOperatorName); } }
    public string IdentificationSessionGuid { get { return this.GetValue(Columns.IdentificationSessionGuid); } }
    public int? PaymentRequestID { get { return this.GetIntValue(Columns.PaymentRequestID); } }
    public int? TransactionID { get { return this.GetIntValue(Columns.TransactionID); } }
    public string SessionCreated 
    { 
      get 
      { 
        DateTime? date =  this.GetDateValue(Columns.SessionCreated);
        if (!date.HasValue)
          return string.Empty;

        int seconds = (int)(DateTime.Now - date.Value).TotalSeconds;
        if (seconds < 60)
          return string.Format(".{0}", seconds);

        int minutes = (int)(DateTime.Now - date.Value).TotalMinutes;
        if (minutes < 60)
        {
          seconds = (int)Math.Floor((double)seconds / (double)minutes);
          return string.Format("{0}.{1}", minutes, seconds);
        }

        int hours = (int)(DateTime.Now - date.Value).TotalMinutes;
        if(hours < 24)
        {
          minutes = (int)Math.Floor((double)minutes / (double)hours);
          return string.Format("{0}h{1}m", hours, minutes);
        }

        int days = (int)(DateTime.Now - date.Value).TotalDays;
        hours = (int)Math.Floor((double)hours / days);
        return string.Format("{0}d{1}h", days, hours);
      } 
    }


    public string PaymentStatus
    {
      get
      {
        string PaymentStatusID = this.GetValue(Columns.PaymentStatusID);
        switch (PaymentStatusID)
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



    public EntranceTableAndroid(DataRow row)
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
      if (Int32.TryParse(this._row[(int)type].ToString(), out result))
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
      UserSessionID,
      ServiceName,
      CountryName,
      MobileOperatorName,
      IdentificationSessionGuid,
      PaymentRequestID,
      PaymentStatusID,
      TransactionID,
      SessionCreated
    }

  }
}
