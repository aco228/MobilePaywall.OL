using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class TimelineModel : ModelBase
  {

    private TimelineTable _data = null;
    public TimelineTable Data { get { return this._data; } set { this._data = value; } }

    public TimelineModel() : base() { }

  }

  public class QuickReportModel : ModelBase
  {
    private List<DataRow> _data = null;

    public string UserSession { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.UserSession].ToString() : ""; } }
    public string Identification { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.Identification].ToString() : ""; } }
    public string PaymentReqeustALL { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentReqeustALL].ToString() : ""; } }
    public string PaymentRequestCOMPLETE { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentRequestCOMPLETE].ToString() : ""; } }
    public string PaymentRequestEXISTS { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentRequestEXISTS].ToString() : ""; } }
    public string PaymentRequestFAILED { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentRequestFAILED].ToString() : ""; } }
    public string PaymentALL { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentALL].ToString() : ""; } }
    public string PaymentSUCCESS { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentSUCCESS].ToString() : ""; } }
    public string PaymentFAILED { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentFAILED].ToString() : ""; } }
    public string PaymentPENDING { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentPENDING].ToString() : ""; } }
    public string PaymentCANCEL { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.PaymentCANCEL].ToString() : ""; } }
    public string TransactionNEW { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.TransactionNEW].ToString() : ""; } }
    public string TransactionSUBSEQUENTS { get { return this._data != null ? this._data[0][(int)QuickReportModelColumns.TransactionSUBSEQUENTS].ToString() : ""; } }

    public QuickReportModel(List<DataRow> rows)
      : base()
    {
      this._data = rows;
    }

  }

  public enum QuickReportModelColumns
  {
    ID,
    UserSession,
    Identification,
    PaymentReqeustALL,
    PaymentRequestCOMPLETE,
    PaymentRequestEXISTS,
    PaymentRequestFAILED,
    PaymentALL,
    PaymentSUCCESS,
    PaymentFAILED,
    PaymentPENDING,
    PaymentCANCEL,
    TransactionNEW,
    TransactionSUBSEQUENTS,
  }

}
