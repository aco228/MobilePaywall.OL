using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class ReportModel : ModelBase
  {
    private string _paywallGuid = string.Empty;
    private string _userSessionID = string.Empty;
    private string _cashflowGuid = string.Empty;
    private string _pxid = string.Empty;
    private string _date = string.Empty;
    private string _ip = string.Empty;

    public string PaywallGuid { get { return this._paywallGuid; } set { this._paywallGuid = value; } }
    public string UserSessionID { get { return this._userSessionID; } set { this._userSessionID = value; } }
    public string CashflowGuid { get { return this._cashflowGuid; } set { this._cashflowGuid = value; } }
    public string Pxid { get { return this._pxid; } set { this._pxid = value; } }
    public string Date { get { return this._date; } set { this._date = value; } }
    public string IP { get { return this._ip; } set { this._ip = value; } }

  }
}
