using MobilePaywall.Direct;
using MobilePaywall.Ol.Core.Data;
using MobilePaywall.Ol.Core.Database;
using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Managers
{
  public class WebLogManager
  {

    private List<WebLogTable> _result = null;

    public List<WebLogTable> Result { get { return this._result; } }
    public int Count { get { return this._result == null ? 0 : this._result.Count; } }

    public WebLogManager()
    { }

    public List<WebLogTable> Query(WebLogTableData data)
    {
      string command = "";
      #region # sql command #
      command = "SELECT  "
                + "wl.WebLogID, "
                + "wl.Date, "
                + "wl.Logger, "
                + "wl.Message, "
                + "wl.Exception, "
                + "wl.Method, "
                + "wl.Line "
                + "FROM MobilePaywallLog.log.WebLog AS wl "
                + "WHERE  "
                + "wl.Message LIKE '%" + data.SessionGuid + "%' "
                + "AND wl.Date > '" + data.From + "' "
                + "AND wl.Date < '" + data.To + "' "
                + " ORDER BY wl.WebLogID ASC;";
      #endregion

      this._result = new List<WebLogTable>();
      MobilePaywallLogDirect database = MobilePaywallLogDirect.Instance;
      database.SetTimeout(0);
      DataTable table = database.Load(command);
      if (table == null)
        return this._result;


      foreach (DataRow row in table.Rows)
      {
        WebLogTable tableRow = new WebLogTable();
        tableRow.ID = row[(int)WebLogColumns.ID].ToString();
        tableRow.Date = row[(int)WebLogColumns.Date].ToString();
        tableRow.Logger = row[(int)WebLogColumns.Logger].ToString();

        tableRow.Message = row[(int)WebLogColumns.Message].ToString();
        string exeption = row[(int)WebLogColumns.Exception].ToString();
        if (!string.IsNullOrEmpty(exeption))
          tableRow.Message += Environment.NewLine + Environment.NewLine + "<<<<<<<<<<<<<<<<: EXCEPTION:" + Environment.NewLine + exeption;

        tableRow.Method = row[(int)WebLogColumns.Method].ToString();
        tableRow.Line = row[(int)WebLogColumns.Line].ToString();
        this._result.Add(tableRow);
      }

      return this._result;
    }

    public List<WebLogTable> Query2(WebLogTableData data)
    {
      string command = "";
      #region # sql command #

      command = " SELECT  " 
                + "wl.WebLogID, "
                + "wl.Date, "
                + "wl.Logger, "
                + "wl.Message, "
                + "wl.Exception, "
                + "wl.Method, "
                + "wl.Line " 
              + " FROM MobilePaywallLog.log.WebLogUserSessionMap AS map " 
              + " LEFT OUTER JOIN MobilePaywallLog.log.WebLog AS wl ON map.WebLogID=wl.WebLogID " 
              + " WHERE map.UserSessionID="+data.SessionID+" AND " 
              + " map.Created >= '"+ data.From +"' AND map.Created <= '"+ data.To +"' " 
              + " ORDER BY wl.WebLogID ASC; ";

      #endregion

      this._result = new List<WebLogTable>();
      MobilePaywallLogDirect database = MobilePaywallLogDirect.Instance;
      database.SetTimeout(0);
      DataTable table = database.Load(command);
      if (table == null)
        return this._result;

      foreach (DataRow row in table.Rows)
      {
        WebLogTable tableRow = new WebLogTable();
        tableRow.ID = row[(int)WebLogColumns.ID].ToString();
        tableRow.Date = row[(int)WebLogColumns.Date].ToString();
        tableRow.Logger = row[(int)WebLogColumns.Logger].ToString();

        tableRow.Message = row[(int)WebLogColumns.Message].ToString();
        string exeption = row[(int)WebLogColumns.Exception].ToString();
        if (!string.IsNullOrEmpty(exeption))
          tableRow.Message += Environment.NewLine + Environment.NewLine + "<<<<<<<<<<<<<<<<: EXCEPTION:" + Environment.NewLine + exeption;

        tableRow.Method = row[(int)WebLogColumns.Method].ToString();
        tableRow.Line = row[(int)WebLogColumns.Line].ToString();
        this._result.Add(tableRow);
      }

      return this._result;
    }

    public List<WebLogTable> QueryLogDatabase(WebLogTableData data)
    {
      DirectDatabaseMsSql database = MobilePaywallLogDirect.Instance;

      // first we check if data is stored in log database
      int? check = database.LoadInt(string.Format(@"
        SELECT COUNT(*) FROM MobilePaywallLog.log.WebLog AS log
        LEFT OUTER JOIN MobilePaywallLog.log.WebLogUserSessionMap AS map ON map.WebLogID=log.WebLogID
        WHERE map.UserSessionID={0}", data.SessionID));

      if (!check.HasValue || check.Value == 0)
        database = MobilePaywallBackupDirect.Instance;


      string command = "";
      #region # sql command #

      command = " SELECT  "
                + "wl.WebLogID, "
                + "wl.Date, "
                + "wl.Logger, "
                + "wl.Message, "
                + "wl.Exception, "
                + "wl.Method, "
                + "wl.Line "
              + " FROM MobilePaywall.log.WebLogUserSessionMap AS map "
              + " LEFT OUTER JOIN MobilePaywall.log.WebLog AS wl ON map.WebLogID=wl.WebLogID "
              + " WHERE map.UserSessionID=" + data.SessionID + " AND "
              + " map.Created >= '" + data.From + "' AND map.Created <= '" + data.To + "' "
              + " ORDER BY wl.WebLogID ASC; ";

      #endregion

      this._result = new List<WebLogTable>();
      database.SetTimeout(0);
      DataTable table = database.Load(command);
      if (table == null)
        return this._result;

      foreach (DataRow row in table.Rows)
      {
        WebLogTable tableRow = new WebLogTable();
        tableRow.ID = row[(int)WebLogColumns.ID].ToString();
        tableRow.Date = row[(int)WebLogColumns.Date].ToString();
        tableRow.Logger = row[(int)WebLogColumns.Logger].ToString();

        tableRow.Message = row[(int)WebLogColumns.Message].ToString();
        string exeption = row[(int)WebLogColumns.Exception].ToString();
        if (!string.IsNullOrEmpty(exeption))
          tableRow.Message += Environment.NewLine + Environment.NewLine + "<<<<<<<<<<<<<<<<: EXCEPTION:" + Environment.NewLine + exeption;

        tableRow.Method = row[(int)WebLogColumns.Method].ToString();
        tableRow.Line = row[(int)WebLogColumns.Line].ToString();
        this._result.Add(tableRow);
      }

      return this._result;
    }

  }
}
