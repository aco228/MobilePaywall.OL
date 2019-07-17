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
  public class OverlayLogManager
  {
    private List<WebLogTable> _result = null;
    public int Count { get { return this._result == null ? 0 : this._result.Count; } }

    public List<WebLogTable> Result { get { return this._result; } }

    public OverlayLogManager() : base() { }

    
    public List<WebLogTable> Query(WebLogTableData data)
    {
      string command = "";
      #region # sql command #
      command = " SELECT  " +
                " FrameLogID AS 'ID', " +
                " Created AS 'Date', " +
                " Code AS 'Code', " +
                " Text AS 'Text',  " +
                " Scenario, " +
                " ConfigurationID, " +
                " CarrierID, " +
                " CountryID, " +
                " TemplateID " +
                " FROM Clickenzi.FrameLog " +
                " WHERE Pxid="+ data.SessionID +" " +
                " AND Created >='" + data.From+"' AND Created <= '" + data.To + "' " + 
                " ORDER BY FrameLogID DESC; ";
      #endregion

      this._result = new List<WebLogTable>();

      ClickenziDirect db = ClickenziDirect.Instance;
      DataTable table = db.Load(command);
      if (table == null)
        return this._result;

      foreach(DataRow row in table.Rows)
      {
        WebLogTable tableRow = new WebLogTable();

        tableRow.ID = !string.IsNullOrEmpty(row[(int)OverlayColumns.ID].ToString()) ? row[(int)OverlayColumns.ID].ToString() : (string)null;
        tableRow.Date = !string.IsNullOrEmpty(row[(int)OverlayColumns.Created].ToString()) ? row[(int)OverlayColumns.Created].ToString() : (string)null;
        tableRow.Logger = !string.IsNullOrEmpty(row[(int)OverlayColumns.Code].ToString()) ? row[(int)OverlayColumns.Code].ToString() : (string)null;
        tableRow.Message = "Scenario: " + (!string.IsNullOrEmpty(row[(int)OverlayColumns.Scenario].ToString()) ? row[(int)OverlayColumns.Scenario].ToString() : "") + "\r\n" +
                           "Configuration: " + (!string.IsNullOrEmpty(row[(int)OverlayColumns.Configuration].ToString()) ? row[(int)OverlayColumns.Configuration].ToString() : "") + "\r\n" +
                           "Carrier: " + (!string.IsNullOrEmpty(row[(int)OverlayColumns.Carrier].ToString()) ? row[(int)OverlayColumns.Carrier].ToString() : "") + "\r\n" +
                           "Country: " + (!string.IsNullOrEmpty(row[(int)OverlayColumns.Country].ToString()) ? row[(int)OverlayColumns.Country].ToString() : "") + "\r\n" +
                           "Template: " + (!string.IsNullOrEmpty(row[(int)OverlayColumns.Template].ToString()) ? row[(int)OverlayColumns.Template].ToString() : "") + "\r\n" +
                           (!string.IsNullOrEmpty(row[(int)OverlayColumns.Text].ToString()) ? row[(int)OverlayColumns.Text].ToString() : "");

        this._result.Add(tableRow);
      }

      return this._result;
    }


  }
}
