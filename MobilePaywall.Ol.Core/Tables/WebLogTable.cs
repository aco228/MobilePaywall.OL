using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{
  public class WebLogTable
  {
    
    private string _id = "";
    private string _date = "";
    private string _logger = "";
    private string _message = "";
    private string _method = "";
    private string _line = "";

    public string ID { get { return this._id; } set { this._id = value; } }
    public string Date { get { return this._date; } set { this._date = value; } }
    public string Logger { get { return this._logger; } set { this._logger = value; } }
    public string Message { get { return this._message; } set { this._message = value; } }
    public string Method { get { return this._method; } set { this._method = value; } }
    public string Line { get { return this._line; } set { this._line = value; } }
    
  }

  public enum WebLogColumns
  {
    ID,
    Date,
    Logger,
    Message,
    Exception,
    Method,
    Line
  }

  public enum CashflowColumns
  {
    ID,
    GroupKey,
    Thread,
    Message,
    Date
  }

  public enum OverlayColumns
  {
    ID,
    Created,
    Code,
    Text,
    Scenario,
    Configuration,
    Carrier,
    Country,
    Template
  }
}
