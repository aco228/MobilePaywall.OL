using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  // SUMMARY: Statistic model  ( probably not in use )
  public class StatisticModel : ModelBase
  {
    private StatisticTable _table = null;
    private DateTime _from = DateTime.Now;
    private DateTime _to = DateTime.Now;

    public StatisticTable Table { get { return this._table; } set { this._table = value; } }
    public DateTime From { get { return this._from; } set { this._from = value; } }
    public DateTime To { get { return this._to; } set { this._to = value; } }
    public string FromString { get { return this._from.ToString("yyyy-MM-dd - HH:mm:ss"); } }
    public string ToString { get { return this._to.ToString("yyyy-MM-dd - HH:mm:ss"); } }

    public StatisticModel(string from, string to) : base() 
    {
      this._from = DateTime.Parse(from);
      this._to = DateTime.Parse(to);
    }
  }

  // SUMMARY: Model for passing data from cashflow to the view
  public class CashflowStatisticModel : ModelBase
  {
    private List<CashflowBehaviorModelTable> _data = null;

    public List<CashflowBehaviorModelTable> Data { get { return this._data; } }

    public CashflowStatisticModel(List<CashflowBehaviorModelTable> data)
      : base()
    {
      this._data = data;
    }
  }
  
}
