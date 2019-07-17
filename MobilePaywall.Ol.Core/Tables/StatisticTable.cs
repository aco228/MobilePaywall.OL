using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{
  public class StatisticTable
  {
    private UserSessionStatisticTable _userSession = null;
    private List<UserSessionStatisticTable> _userSessionHours = null;
    private List<UserSessionCountryStatisticTable> _userSessionCountry = null;
    private List<UserSessionCountryStatisticTable> _transactionCountry = null;
    private IterationStatisticTable _paymentReqeusts = null;
    private IterationStatisticTable _payments = null;

    public UserSessionStatisticTable UserSession { get { return this._userSession; } set { this._userSession = value; } }
    public List<UserSessionStatisticTable> UserSessionHours { get { return this._userSessionHours; } set { this._userSessionHours = value; } }
    public List<UserSessionCountryStatisticTable> UserSessionCountry { get { return this._userSessionCountry; } set { this._userSessionCountry = value; } }
    public List<UserSessionCountryStatisticTable> TransactionCountry { get { return this._transactionCountry; } set { this._transactionCountry = value; } }
    public IterationStatisticTable PaymentReqests { get { return this._paymentReqeusts; } set { this._paymentReqeusts = value; } }
    public IterationStatisticTable Payments { get { return this._payments; } set { this._payments = value; } }

    public StatisticTable() { }
  }

  public class UserSessionStatisticTable
  {
    private int _userSession = -1;
    private int _lookup = -1;
    private int _paymentReqeust = -1;
    private int _payment = -1;
    private int _subsequent = -1;
    private int _transaction = -1;

    public int UserSession { get { return this._userSession; } set { this._userSession = value; } }
    public int Lookup { get { return this._lookup; } set { this._lookup = value; } }
    public int PaymentRequest { get { return this._paymentReqeust; } set { this._paymentReqeust = value; } }
    public int Payment { get { return this._payment; } set { this._payment = value; } }
    public int Subsequent { get { return this._subsequent; } set { this._subsequent = value; } }
    public int Transaction { get { return this._transaction; } set { this._transaction = value; } }

    public enum Columns
    {
      UserSession,
      Lookup,
      PaymentReqeust,
      Payment,
      Subsequent,
      Transaction
    }
  }

  public class UserSessionCountryStatisticTable
  {
    private string _country = string.Empty;
    private int _num = -1;

    public string Country { get { return this._country; } set { this._country = value; } }
    public int Num { get { return this._num; } set { this._num = value; } }

    public enum Columns
    {
      Num,
      Country
    }
  }

  public class IterationStatisticTable
  {
    private List<string> _names = null;
    private List<int> _values = null;

    public List<string> Names { get { return this._names; } set { this._names = value; } }
    public List<int> Values { get { return this._values; } set { this._values = value; } }

  }

  public class TimelineTable
  {
    private string _title = string.Empty;
    private List<string> _time = null;
    private List<string> _userSession = null;
    private List<string> _identification = null;
    private List<string> _transaction = null;
    private List<string> _subsequent = null;

    public string Title { get { return this._title; } set { this._title = value; } }
    public List<string> Time { get { return this._time; } set { this._time = value; } }
    public List<string> UserSession { get { return this._userSession; } set { this._userSession = value; } }
    public List<string> Identification { get { return this._identification; } set { this._identification = value; } }
    public List<string> Transaction { get { return this._transaction; } set { this._transaction = value; } }
    public List<string> Subsequent { get { return this._subsequent; } set { this._subsequent = value; } }

    public TimelineTable()
    {
      this._time = new List<string>();
      this._userSession = new List<string>();
      this._identification = new List<string>();
      this._transaction = new List<string>();
      this._subsequent = new List<string>();
    }

    public enum Columns
    {
      Time,
      UserSession,
      Identification,
      Transaction,
      Subsequent
    }
  }
}
