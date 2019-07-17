using MobilePaywall.Data;
using MobilePaywall.Ol.Core.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Data
{
  public class EntranceTableData : DataObjectBase
  {
    private string _limit = "";
    private string _from = "";
    private string _to = "";
    private string _country = "";
    private List<string> _countries = null;
    private string _mobileOperator = "";
    private string _service = "";
    private List<string> _services = null;
    private string _msisdn = "";
    private List<string> _msisdns = null;
    private string _pxid = "";
    private List<string> _pxids = null;
    private string _ip = "";
    private string _referrerContains = "";
    private List<string> _referrerContainsList = null;
    private List<string> _ips = null;
    private string _useOLReference = "";
    private string _olRefferer = "";
    private string _paymentStatus = "";
    private string _paymentRequest = "";
    private string _payment = "";
    private string _transaction = "";
    private string _userSessionReference = "";
    private string _statisticGroupBy = "";
    private bool _transactionUseUserSessionDate = false;
    private string _transactionUserSessionFrom = string.Empty;
    private string _transactionUserSessionTo = string.Empty;
    private EntranceSearchType _paymentRequestSearchType = EntranceSearchType.Default;
    private EntranceSearchType _paymentSearchType = EntranceSearchType.Default;
    private EntranceSearchType _transactionSearchType = EntranceSearchType.Default;
    private string _androidSessionID = string.Empty;
    private int? _androidClientSessionID = null;
    private string _useSequentalSearch = string.Empty;
    private string _olCacheID = string.Empty;
    private string _returnView = "_DataNew";

    public string Limit { get { return this._limit != null ? this._limit : ""; } set { this._limit = value; } }
    public string From { get { return this._from != null ? this._from : ""; } set { this._from = value; } }
    public string To { get { return this._to != null ? this._to : ""; } set { this._to = value; } }
    public string Country { get { return this._country != null ? this._country : ""; } set { this._country = value; } }
    public List<string> Countries { get { return this._countries == null ? new List<string>() : this._countries; } }
    public string MobileOperator { get { return this._mobileOperator; } set { this._mobileOperator = value; } }
    public string Service { get { return this._service != null ? this._service : ""; } set { this._service = value; } }
    public List<string> Services { get { return this._services == null ? new List<string>() : this._services; } }
    public string Msisdn { get { return this._msisdn != null ? this._msisdn : ""; } set { this._msisdn = value; } }
    public List<string> Msisdns { get { return this._msisdns == null ? new List<string>() : this._msisdns; } }
    public string Pxid { get { return this._pxid != null ? this._pxid : ""; } set { this._pxid = value; } }
    public List<string> Pxids { get { return this._pxids == null ? new List<string>() : this._pxids; } }
    public string IP { get { return this._ip != null ? this._ip : ""; } set { this._ip = value; } }
    public string ReferrerContains { get { return this._referrerContains; } set { this._referrerContains = value; } }
    public List<string> ReferrerContainsList { get { return this._referrerContainsList != null ? this._referrerContainsList : new List<string>(); } }
    public List<string> IPS { get { return this._ips == null ? new List<string>() : this._ips; } }
    public string UseOLReference { get { return this._useOLReference != null ? this._useOLReference : ""; } set { this._useOLReference = value; } }
    public string OLRefferer { get { return this._olRefferer != null ? this._olRefferer : ""; } set { this._olRefferer = value; } }
    public string PaymentStatus { get { return this._paymentStatus != null ? this._paymentStatus : ""; } set { this._paymentStatus = value; } }
    public string PaymentRequest { get { return this._paymentRequest != null ? this._paymentRequest : ""; } set { this._paymentRequest = value; } }
    public string Payment { get { return this._payment != null ? this._payment : ""; } set { this._payment = value; } }
    public string Transaction { get { return this._transaction != null ? this._transaction : ""; } set { this._transaction = value; } }
    public string UserSessionReference { get { return this._userSessionReference; } set { this._userSessionReference = value; } }
    public string StatisticGroupBy { get { return this._statisticGroupBy; } set { this._statisticGroupBy = value; } }
    public bool TransactionUseUserSessionDate { get { return this._transactionUseUserSessionDate; } }
    public string TransactionUserSessionDateFrom { get { return this._transactionUserSessionFrom; } set { this._transactionUserSessionFrom = value; } }
    public string TransactionUserSessionDateTo { get { return this._transactionUserSessionTo; } set { this._transactionUserSessionTo = value; } }
    public string AndroidID { get { return this._androidSessionID; } set { this._androidSessionID = value; } }
    public int? AndroidClientSession { get { return this._androidClientSessionID; } set { this._androidClientSessionID = value; } }
    public EntranceSearchType PaymentRequestSearchType { get { return this._paymentRequestSearchType; } }
    public EntranceSearchType PaymentSearchType { get { return this._paymentSearchType; } }
    public EntranceSearchType TransactionSearchType { get { return this._transactionSearchType; } }
    public string SequentialSearch { get { return this._useSequentalSearch; } set { this._useSequentalSearch = value; } }
    public bool UseSequentialSearch { get { return !string.IsNullOrEmpty(this._useSequentalSearch) && this._useSequentalSearch.Equals("1"); } }
    public string OLCacheID { get { return this._olCacheID; } set { this._olCacheID = value ; } }
    public string ReturnView { get { return this._returnView; } set { this._returnView = value; } }

    public EntranceTableData() : base() { }

    public void Initialize()
    { }

    public override bool Validation()
    {
      Int32.TryParse(this.Limit, out this._selectTop);
      DateTime _temp;
      if(!DateTime.TryParse(this._from, out _temp))
      {
        this._hasError = true;
        this._errorMessage = "Wrong date format";
        return this._hasError;
      }
      
      int _tempNum;
      if(!Int32.TryParse(this.UseOLReference, out _tempNum) && _tempNum > 1)
      {
        this._hasError = true;
        this._errorMessage = "Wrong checkbox format";
        return this._hasError;
      }

      this._paymentRequestSearchType = this.ConvertToSerachType(this.PaymentRequest);
      this._paymentSearchType = this.ConvertToSerachType(this.Payment);
      this._transactionSearchType = this.ConvertToSerachType(this.Transaction);

      this._countries = this.ConvertToList(this._country, this._countries);
      this._services = this.ConvertToList(this._service, this._services);
      this._pxids = this.ConvertToList(this._pxid, this._pxids);
      this._ips = this.ConvertToList(this._ip, this._ips);
      this._msisdns = this.ConvertToList(this._msisdn, this._msisdns);
      this._referrerContainsList = this.ConvertToList(this._referrerContains, this._referrerContainsList);

      if(!string.IsNullOrEmpty(this._transactionUserSessionFrom) && !string.IsNullOrEmpty(this._transactionUserSessionTo) &&
        DateTime.TryParse(this._transactionUserSessionFrom, out _temp) && DateTime.TryParse(this._transactionUserSessionTo, out _temp))
        this._transactionUseUserSessionDate = true;

      if(!string.IsNullOrEmpty(this._androidSessionID))
      {
        int tempid = -1;
        if (Int32.TryParse(this._androidSessionID, out tempid))
          this._androidClientSessionID = tempid;
      }

      return false;
    }

    private List<string> ConvertToList(string var, List<string> list)
    {
      if (string.IsNullOrEmpty(var)) return null;
      string[] data = var.Split('|');
      list = new List<string>();
      foreach (string d in data)
      {
        if (string.IsNullOrEmpty(d)) continue;
        list.Add(d.Trim());
      }
      return list;
    }    

    public string PrepareReffererParts()
    {
      string _base = " us.Referrer LIKE ";
      if(this._referrerContainsList == null || this._referrerContainsList.Count == 0)
        return " OR " + _base + "''";

      string returnString = " OR ( ";
      bool first = true;

      foreach(string part in this._referrerContainsList)
      {
        string or = !first ? " OR " : "";
        first = false;

        returnString += or + string.Format(" {0} '%?{1}' ", _base, part); // only argument on the front
        returnString += string.Format(" OR {0} '%{1}&%' ", _base, part); // in the middle
        returnString += string.Format(" OR {0} '%&{1}' ", _base, part); // on the end
      }

      returnString += " ) ";
      return returnString;
    }
    
  }

  
}
