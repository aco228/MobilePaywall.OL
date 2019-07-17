using MobilePaywall.Ol.Core.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Data
{
  public class WebLogTableData : DataObjectBase
  {
    private string _sessionID = "";
    private string _sessionGuid = "";
    private string _dateFrom = "";

    public string SessionID { get { return this._sessionID; } set { this._sessionID = value; } }
    public string SessionGuid { get { return this._sessionGuid; } set { this._sessionGuid = value; } }
    public string DateFrom { get { return this._dateFrom; } set { this._dateFrom = value; } }
    //public string From { get { return !string.IsNullOrEmpty(this._dateFrom) ? this.ConvertDate(DateTime.ParseExact(this._dateFrom, "yyyy-MM-dd HH:mm:ss:fffffff", CultureInfo.InvariantCulture).AddMinutes(-10)) : this.ConvertDate(DateTime.Now); } }
    //public string To { get { return !string.IsNullOrEmpty(this._dateFrom) ? this.ConvertDate(DateTime.ParseExact(this._dateFrom, "yyyy-MM-dd HH:mm:ss:fffffff", CultureInfo.InvariantCulture).AddMinutes(10)) : this.ConvertDate(DateTime.Now); } }

    public string From { get { return !string.IsNullOrEmpty(this._dateFrom) ? this.ConvertDate(DateTime.Parse(this._dateFrom).AddMinutes(-3)) : this.ConvertDate(DateTime.Now); } }
    public string To { get { return !string.IsNullOrEmpty(this._dateFrom) ? this.ConvertDate(DateTime.Parse(this._dateFrom).AddHours(2)) : this.ConvertDate(DateTime.Now); } }

    public override bool Validation()
    {
      return true;
    }
  }
}
