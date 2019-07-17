using MobilePaywall.Data;
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
  public class StatisticManager : MobilePaywallDatabase
  {
    public StatisticManager() : base() { }

    // SUMMARY: only this is in use! TIMELINE
    public TimelineTable LoadTimeline(EntranceTableData data)
    {
      string command = "";

      #region # prepare reference days #

      DateTime startingDate = DateTime.Parse(data.From),
               endingDate = DateTime.Parse(data.To),
               referenceStartDate = DateTime.Now,
               referenceEndDate = DateTime.Now;

      switch (data.StatisticGroupBy)
      {
        case "HOUR":
          referenceStartDate = new DateTime(startingDate.Year, startingDate.Month, startingDate.Day, 0, 0, 0);
          startingDate = startingDate.AddDays(1);
          referenceEndDate = new DateTime(startingDate.Year, startingDate.Month, startingDate.Day, 0, 0, 0);
          break;
        case "WEEK":
          startingDate = startingDate.AddDays( 1 - (int)startingDate.DayOfWeek );
          referenceStartDate = new DateTime(startingDate.Year, startingDate.Month, startingDate.Day, 0, 0, 0);
          startingDate = startingDate.AddDays(7);
          referenceEndDate = new DateTime(startingDate.Year, startingDate.Month, startingDate.Day, 0, 0, 0);
          data.StatisticGroupBy = "DAY";
          break;
        case "MONTH":
          referenceStartDate = new DateTime(startingDate.Year, startingDate.Month, 1, 0, 0, 0);
          startingDate = startingDate.AddMonths(1);
          referenceEndDate = new DateTime(startingDate.Year, startingDate.Month, 1, 0, 0, 0);
          data.StatisticGroupBy = "DAY";
          break;
        case "YEAR":
          referenceStartDate = new DateTime(startingDate.Year, 1, 1, 0, 0, 0);
          startingDate = startingDate.AddYears(1);
          referenceEndDate = new DateTime(startingDate.Year, 1, 1, 0, 0, 0);
          data.StatisticGroupBy = "MONTH";
          break;
      }

      #endregion

      string startDate = data.ConvertDate(referenceStartDate),
             endDate = data.ConvertDate(referenceEndDate);

      #region # sql command #

      command = " SELECT us._time, us.num AS 'us_num', pr.num AS 'pr_num', t.num AS 't_num', st.num AS 'st_num' " +
                " FROM " +
                " (  " +
                " 	SELECT COUNT(*) AS num,  DATEPART(" + data.StatisticGroupBy + ", us.Created) AS _time FROM MobilePaywall.core.UserSession AS us, " +
                //" 	LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                //" 	WHERE us.Created >= '" + startDate + "' AND us.Created <= '" + endDate + "' " +
                "	  ( SELECT UserSessionGuid FROM MobilePaywall.core.UserSession AS us WHERE us.Created >= '" + startDate + "' AND us.Created <= '" + endDate + "' GROUP BY us.UserSessionGuid ) AS us1, " +
                "	  ( SELECT ServiceID, Name, Description FROM MobilePaywall.core.Service AS s ) AS s " +
                "   WHERE us1.UserSessionGuid=us.UserSessionGuid AND us.ServiceID=s.ServiceID AND us.Created >= '" + startDate + "' AND us.Created <= '" + endDate + "'  " +
                "               AND ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("s.Name LIKE '%{0}%'", data.Services) + " ) " +
                "               AND ( '" + data.MobileOperator + "' = -1 OR us.MobileOperatorID = '" + data.MobileOperator + "' ) " +
                "               AND ( '" + data.Country + "' = '' " + DataObjectBase.PrepareList("s.Description LIKE UPPER('{0}') + '%' ", data.Countries) + " ) " +
                " 	GROUP BY DATEPART(" + data.StatisticGroupBy + ", us.Created) " +
                " ) AS us " +
                " LEFT OUTER JOIN " +
                " ( " +
                " 	SELECT COUNT(*) AS num, DATEPART(" + data.StatisticGroupBy + ", pr.Created) AS _time FROM MobilePaywall.core.PaymentRequest AS pr " +
                " 	LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON pr.UserSessionID=us.UserSessionID " +
                " 	LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                " 	WHERE pr.Created >= '" + startDate + "' AND pr.Created <= '" + endDate + "' " +
                "               AND ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("s.Name LIKE '%{0}%'", data.Services) + " ) " +
                "               AND ( '" + data.MobileOperator + "' = -1 OR us.MobileOperatorID = '" + data.MobileOperator + "' ) " +
                "               AND ( '" + data.Country + "' = '' " + DataObjectBase.PrepareList("s.Description LIKE UPPER('{0}') + '%' ", data.Countries) + " ) " +
                " 	GROUP BY DATEPART(" + data.StatisticGroupBy + ", pr.Created)	 " +
                " ) AS pr ON us._time=pr._time " +
                " LEFT OUTER JOIN " +
                " ( " +
                " 	SELECT COUNT(*) AS num, DATEPART(" + data.StatisticGroupBy + ", t.Created) AS _time FROM MobilePaywall.core.[Transaction] AS t " +
                " 	LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID " +
                " 	LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON p.PaymentRequestID=pr.PaymentRequestID " +
                " 	LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON pr.UserSessionID=us.UserSessionID " +
                " 	LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                " 	WHERE t.Created >= '" + startDate + "' AND t.Created <= '" + endDate + "' AND t.TransactionStatusID=5 " +
                "               AND ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("s.Name LIKE '%{0}%'", data.Services) + " ) " +
                "               AND ( '" + data.MobileOperator + "' = -1 OR us.MobileOperatorID = '" + data.MobileOperator + "' ) " +
                "               AND ( '" + data.Country + "' = '' " + DataObjectBase.PrepareList("s.Description LIKE UPPER('{0}') + '%' ", data.Countries) + " ) " +
                " 		          AND (SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] WHERE PaymentID=p.PaymentID)=1 " +
                " 	GROUP BY DATEPART(" + data.StatisticGroupBy + ", t.Created)	 " +
                " ) AS t ON t._time=us._time " +
                " LEFT OUTER JOIN " +
                " ( " +
                " 	SELECT COUNT(*) AS num, DATEPART(" + data.StatisticGroupBy + ", t.Created) AS _time FROM MobilePaywall.core.[Transaction] AS t " +
                " 	LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID " +
                " 	LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON p.PaymentRequestID=pr.PaymentRequestID " +
                " 	LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON pr.UserSessionID=us.UserSessionID " +
                " 	LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                " 	WHERE t.Created >= '" + startDate + "' AND t.Created <= '" + endDate + "' AND t.TransactionStatusID=5 " +
                "               AND ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("s.Name LIKE '%{0}%'", data.Services) + " ) " +
                "               AND ( '" + data.MobileOperator + "' = -1 OR us.MobileOperatorID = '" + data.MobileOperator + "' ) " +
                "               AND ( '" + data.Country + "' = '' " + DataObjectBase.PrepareList("s.Description LIKE UPPER('{0}') + '%' ", data.Countries) + " ) " +
                " 		          AND (SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] WHERE PaymentID=p.PaymentID)>1 " +
                " 	GROUP BY DATEPART(" + data.StatisticGroupBy + ", t.Created)	 " +
                " ) AS st ON st._time=us._time " +
                " ORDER BY us._time ASC ";

      #endregion

      TimelineTable returnTable = new TimelineTable();
      returnTable.Title = string.Format("Group by {0} in time of {1} - {2}", data.StatisticGroupBy, referenceStartDate.ToString(), referenceEndDate.ToString());
      DataTable table = this.Load(command);
      if (table == null)
        return returnTable;

      foreach(DataRow row in table.Rows)
      {
        returnTable.Time.Add(string.IsNullOrEmpty(row[(int)TimelineTable.Columns.Time].ToString()) ? "0" : row[(int)TimelineTable.Columns.Time].ToString());
        returnTable.UserSession.Add(string.IsNullOrEmpty(row[(int)TimelineTable.Columns.UserSession].ToString()) ? "0" : row[(int)TimelineTable.Columns.UserSession].ToString());
        returnTable.Identification.Add(string.IsNullOrEmpty(row[(int)TimelineTable.Columns.Identification].ToString()) ? "0" : row[(int)TimelineTable.Columns.Identification].ToString());
        returnTable.Transaction.Add(string.IsNullOrEmpty(row[(int)TimelineTable.Columns.Transaction].ToString()) ? "0" : row[(int)TimelineTable.Columns.Transaction].ToString());
        returnTable.Subsequent.Add(string.IsNullOrEmpty(row[(int)TimelineTable.Columns.Subsequent].ToString()) ? "0" : row[(int)TimelineTable.Columns.Subsequent].ToString());
      }

      return returnTable;
    }

    // SUMMARY: QuickReport all in one load
    public List<DataRow> LoadQuickReports(EntranceTableData data)
    {
      string command = "";

      #region # sql command #
      command = " " +
        " DECLARE @result TABLE " +
        " ( " +
          " ID INT,  " +
          " UserSession INT, " +
          " Identification INT, " +
          " PaymentReqeustALL INT, " +
          " PaymentRequestCOMPLETE INT, " +
          " PaymentRequestEXISTS INT, " +
          " PaymentRequestFAILED INT, " +
          " PaymentALL INT, " +
          " PaymentSUCCESS INT, " +
          " PaymentFAILED INT, " +
          " PaymentPENDING INT, " +
          " PaymentCANCEL INT, " +
          " TransactionNEW INT, " +
          " TransactionSUBSEQUENTS INT " +
        " ) " +
        " INSERT INTO @result (ID) VALUES (1); " +
         " " +
        // " --	Getting user sessions " +
        " DECLARE @us TABLE ( UserSessionID INT,  CustomerID INT ); " +
        " INSERT INTO @us  " +
        " SELECT UserSessionID, CustomerID FROM MobilePaywall.core.UserSession AS us " +
        " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
        " WHERE " +
        " ( '"+ data.Country +"' = '' OR s.Description LIKE UPPER('"+ data.Country +"') + '%' ) AND  " +
        " ( '"+ data.Service +"' = '' OR s.Name LIKE '%' + LOWER('"+ data.Service +"') + '%' ) AND " +
        " ( "+ data.MobileOperator +" = -1 OR us.MobileOperatorID = "+ data.MobileOperator +" ) AND  " +
        " ( " + data.UseOLReference + " = 0 OR us.Referrer LIKE '"+ data.OLRefferer +"' + '%' ) AND " +
        " ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) AND " +
        " us.Created >= '"+ data.From +"' AND us.Created <= '"+ data.To +"'; " +
        " UPDATE @result SET UserSession = ( SELECT COUNT(*) FROM @us ) " +
        " UPDATE @result SET Identification = ( SELECT COUNT(*) FROM @us WHERE CustomerID IS NOT NULL); " +
         " " +
        //" --Getting Payment reqeusts " +
        " DECLARE @pr AS TABLE  ( PaymentRequestID INT, StatusID INT )  " +
        " INSERT INTO @pr " +
        " SELECT pr.PaymentRequestID, pr.PaymentRequestStatusID FROM MobilePaywall.core.PaymentRequest AS pr, @us AS us " +
        " WHERE pr.UserSessionID=us.UserSessionID " +
         " " +
        " UPDATE @result SET PaymentReqeustALL = (SELECT COUNT(*) FROM @pr ) WHERE ID=1 " +
        " UPDATE @result SET PaymentRequestCOMPLETE = (SELECT COUNT(*) FROM @pr WHERE StatusID=3 ) WHERE ID=1  " +
        " UPDATE @result SET PaymentRequestEXISTS = (SELECT COUNT(*) FROM @pr WHERE StatusID=5 ) WHERE ID=1  " +
        " UPDATE @result SET PaymentRequestFAILED = (SELECT COUNT(*) FROM @pr WHERE StatusID=4 ) WHERE ID=1  " +
         " " +
        //" --	Getting payments " +
        " DECLARE @p AS Table ( PaymentID INT, StatusID INT ); " +
        " INSERT INTO @p " +
        " SELECT PaymentID, PaymentStatusID FROM MobilePaywall.core.Payment AS p, @pr AS pr " +
        " WHERE p.PaymentRequestID=pr.PaymentRequestID; " +
         " " +
        " UPDATE @result SET PaymentALL = (SELECT COUNT(*) FROM @p )  WHERE ID=1 " +
        " UPDATE @result SET PaymentSUCCESS = (SELECT COUNT(*) FROM @p WHERE StatusID=3 ) WHERE ID=1  " +
        " UPDATE @result SET PaymentFAILED = (SELECT COUNT(*) FROM @p WHERE StatusID=4) WHERE ID=1  " +
        " UPDATE @result SET PaymentCANCEL = (SELECT COUNT(*) FROM @pr WHERE StatusID=5 ) WHERE ID=1  " +
        " UPDATE @result SET PaymentPENDING = (SELECT COUNT(*) FROM @p WHERE StatusID=2 ) WHERE ID=1  " +
         " " +
        //" --	Getting Transactions " +
        " UPDATE @result SET TransactionNEW = ( " +
        " SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] AS t, @p AS p " +
        " WHERE t.PaymentID=p.PaymentID " +
        " ); " +
         " " +
        " SELECT * FROM @result; ";
      #endregion

      DataTable table = this.Load(command);
      List<DataRow> rows = new List<DataRow>();
      foreach (DataRow row in table.Rows)
        rows.Add(row);
      return rows;
    }

    public List<JsonObjectHelper> LoadUserSessions(EntranceTableData data)
    {
      string command = string.Empty;
      #region # command #
      command = "( SELECT COUNT(*) FROM MobilePaywall.core.UserSession AS us " +
                " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                " WHERE " +
                " ( '" + data.Country + "' = '' OR s.Description LIKE UPPER('" + data.Country + "') + '%' ) AND  " +
                " ( '" + data.Service + "' = '' OR s.Name LIKE '%' + LOWER('" + data.Service + "') + '%' ) AND " +
                " ( " + data.MobileOperator + " = -1 OR us.MobileOperatorID = " + data.MobileOperator + " ) AND  " +
                " ( " + data.UseOLReference + " = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) AND " +
                " ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) AND " +
                " us.Created >= '" + data.From + "' AND us.Created <= '" + data.To + "' ) " +
                " UNION ALL " +
                "( SELECT COUNT(*) FROM MobilePaywall.core.UserSession AS us " +
                " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                " WHERE " +
                " ( '" + data.Country + "' = '' OR s.Description LIKE UPPER('" + data.Country + "') + '%' ) AND  " +
                " ( '" + data.Service + "' = '' OR s.Name LIKE '%' + LOWER('" + data.Service + "') + '%' ) AND " +
                " ( " + data.MobileOperator + " = -1 OR us.MobileOperatorID = " + data.MobileOperator + " ) AND  " +
                " ( " + data.UseOLReference + " = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) AND " +
                " ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) AND " +
                " us.Created >= '" + data.From + "' AND us.Created <= '" + data.To + "' AND us.CustomerID IS NOT NULL ) ";
      #endregion

      DataTable table = this.Load(command);
      if (data == null)
        return new List<JsonObjectHelper>();

      List<JsonObjectHelper> result = new List<JsonObjectHelper>();
      result.Add(new JsonObjectHelper("sessions", table.Rows[0][0].ToString()));
      result.Add(new JsonObjectHelper("identified", table.Rows[1][0].ToString()));

      return result;
    }

    // SUMMARY: Load informations about Payment Reqestst
    public List<JsonObjectHelper> LoadPaymentRequests(EntranceTableData data)
    {
      string command = string.Empty;
      #region # sql command #

      command = " SELECT pr.PaymentRequestStatusID, COUNT(*) FROM MobilePaywall.core.PaymentRequest AS pr  " +
                  " LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON pr.UserSessionID=us.UserSessionID " +
                  " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                  " WHERE " +
                  " ( '" + data.Country + "' = '' OR s.Description LIKE UPPER('" + data.Country + "') + '%' ) AND  " +
                  " ( '" + data.Service + "' = '' OR s.Name LIKE '%' + LOWER('" + data.Service + "') + '%' ) AND " +
                  " ( " + data.MobileOperator + " = -1 OR us.MobileOperatorID = " + data.MobileOperator + " ) AND  " +
                  " ( " + data.UseOLReference + " = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) AND " +
                  " ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) AND " +
                  " us.Created >= '" + data.From + "' AND us.Created <= '" + data.To + "' AND us.CustomerID IS NOT NULL  " +
                  " GROUP BY pr.PaymentRequestStatusID ";
      #endregion

      DataTable dataTable = this.Load(command);
      if (dataTable == null)
        return new List<JsonObjectHelper>();

      List<JsonObjectHelper> result = new List<JsonObjectHelper>();
      result.Add(new JsonObjectHelper("Initialized", 0, "1"));   // index 0, row 1
      result.Add(new JsonObjectHelper("Pending", 0, "2"));       // index 1, row 2
      result.Add(new JsonObjectHelper("Complete", 0, "3"));      // index 2, row 3
      result.Add(new JsonObjectHelper("Failure", 0, "4"));       // index 3, row 4
      result.Add(new JsonObjectHelper("PaymentExists", 0, "5")); // index 4, row 5

      foreach (DataRow row in dataTable.Rows)
        foreach (JsonObjectHelper joh in result)
          if (joh.Reference.Equals(row[0].ToString()))
            joh.Value = row[1].ToString();

      return result;
    }

    public List<JsonObjectHelper> LoadPayments(EntranceTableData data)
    {
      string command = string.Empty;
      #region # sql command #

      command = " SELECT p.PaymentStatusID, COUNT(*) FROM MobilePaywall.core.Payment AS p  " +
                  " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON p.PaymentRequestID=pr.PaymentRequestID " +
                  " LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON pr.UserSessionID=us.UserSessionID " +
                  " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                  " WHERE " +
                  " ( '" + data.Country + "' = '' OR s.Description LIKE UPPER('" + data.Country + "') + '%' ) AND  " +
                  " ( '" + data.Service + "' = '' OR s.Name LIKE '%' + LOWER('" + data.Service + "') + '%' ) AND " +
                  " ( " + data.MobileOperator + " = -1 OR us.MobileOperatorID = " + data.MobileOperator + " ) AND  " +
                  " ( " + data.UseOLReference + " = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) AND " +
                  " ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) AND " +
                  " us.Created >= '" + data.From + "' AND us.Created <= '" + data.To + "' AND us.CustomerID IS NOT NULL  " +
                  " GROUP BY p.PaymentStatusID ";
      #endregion

      DataTable dataTable = this.Load(command);
      if (dataTable == null)
        return new List<JsonObjectHelper>();

      List<JsonObjectHelper> result = new List<JsonObjectHelper>();
      result.Add(new JsonObjectHelper("Initialized", 0, "1"));   // index 0, row 1
      result.Add(new JsonObjectHelper("Pending", 0, "2"));       // index 1, row 2
      result.Add(new JsonObjectHelper("Successful", 0, "3"));    // index 2, row 3
      result.Add(new JsonObjectHelper("Failed", 0, "4"));        // index 3, row 4
      result.Add(new JsonObjectHelper("Cancelled", 0, "5"));    // index 4, row 5

      foreach (DataRow row in dataTable.Rows)
        foreach (JsonObjectHelper joh in result)
          if (joh.Reference.Equals(row[0].ToString()))
            joh.Value = row[1].ToString();

      return result;
    }
    
    public List<JsonObjectHelper> LoadTransactions(EntranceTableData data)
    {
      string command = string.Empty;
      #region # sql command #

      command = " ( " +
                    " SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] AS t  " +
                    " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID " +
                    " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON p.PaymentRequestID=pr.PaymentRequestID " +
                    " LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON pr.UserSessionID=us.UserSessionID " +
                    " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                    " WHERE " +
                    " ( '" + data.Country + "' = '' OR s.Description LIKE UPPER('" + data.Country + "') + '%' ) AND  " +
                    " ( '" + data.Service + "' = '' OR s.Name LIKE '%' + LOWER('" + data.Service + "') + '%' ) AND " +
                    " ( " + data.MobileOperator + " = -1 OR us.MobileOperatorID = " + data.MobileOperator + " ) AND  " +
                    " ( " + data.UseOLReference + " = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) AND " +
                    " ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) AND " +
                    " t.Created >= '" + data.From + "' AND t.Created <= '" + data.To + "' AND " +
                    //" (SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] WHERE PaymentID=p.PaymentID AND TransactionStatusID=5) = 1  " +
                    " p.Created > '"+ data.From +"' " +
                  " ) UNION ALL ( " +
                    " SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] AS t  " +
                    " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON t.PaymentID=p.PaymentID " +
                    " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON p.PaymentRequestID=pr.PaymentRequestID " +
                    " LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON pr.UserSessionID=us.UserSessionID " +
                    " LEFT OUTER JOIN MobilePaywall.core.Service AS s ON us.ServiceID=s.ServiceID " +
                    " WHERE " +
                    " ( '" + data.Country + "' = '' OR s.Description LIKE UPPER('" + data.Country + "') + '%' ) AND  " +
                    " ( '" + data.Service + "' = '' OR s.Name LIKE '%' + LOWER('" + data.Service + "') + '%' ) AND " +
                    " ( " + data.MobileOperator + " = -1 OR us.MobileOperatorID = " + data.MobileOperator + " ) AND  " +
                    " ( " + data.UseOLReference + " = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) AND " +
                    " ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) AND " +
                    " t.Created >= '" + data.From + "' AND t.Created <= '" + data.To + "' AND " +
                    //" (SELECT COUNT(*) FROM MobilePaywall.core.[Transaction] WHERE PaymentID=p.PaymentID AND TransactionStatusID=5) > 1  " +
                    " p.Created < '" + data.From + "' " +
                  ")";

      #endregion

      DataTable table = this.Load(command);
      List<JsonObjectHelper> result = new List<JsonObjectHelper>();
      result.Add(new JsonObjectHelper("transactions", table.Rows[0][0].ToString(), "tran"));
      result.Add(new JsonObjectHelper("subsequents", table.Rows[1][0].ToString(), "sub"));

      return result;
    }

  }

  public class JsonObjectHelper
  {
    private string _name = string.Empty;
    private string _value = string.Empty;
    private string _reference = string.Empty;

    public string Name { get { return this._name; } }
    public string Value { get { return this._value; } set { this._value = value; } }
    public string Reference { get { return this._reference; } }
    
    public JsonObjectHelper(string name, string value, string reference = "")
    {
      this._name = name;
      this._reference = reference;
      int temp;
      if (Int32.TryParse(value, out temp))
        this._value = value;
      else
        this._value = "\"" + value + "\"";
      
    }

    public JsonObjectHelper(string name, int value, string reference = "")
    {
      this._name = name;
      this._reference = reference;
      this._value = value.ToString();
    }

    public static string Json(List<JsonObjectHelper> data)
    {
      string result = "{";
      foreach(JsonObjectHelper d in data)
      {
        if (!result.Equals("{")) result += ",";
        result += string.Format("\"{0}\":{1}", d.Name, d.Value);
      }

      return result + "}";
    }

  }

}
