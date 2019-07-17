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
  public class CashflowLogManager
  {
    private List<WebLogTable> _result = null;
    public List<WebLogTable> Result { get { return this._result; } }
    public int Count { get { return this._result == null ? 0 : this._result.Count; } }

    public CashflowLogManager()
    { }

    // SUMMARY: Sql for getting all records for the click
    public List<WebLogTable> Query(WebLogTableData data, string UseRefence)
    {
      string command = "";
      #region # sql command #
      if(UseRefence.Equals("true"))
      {
        command = " SELECT " +
                    " rm.RawMessageID AS 'ID', " +
                    " rm.GroupKey AS 'Logger', " +
                    " rm.Thread, " +
                    " rm.RawMessageData AS 'Message', " +
                    " rm.Created AS 'Date'" +
                    " FROM [Cashflow].[log].[RawMessage] AS rm " +
                    " INNER JOIN [Cashflow].[core].[ActionContext] AS ac ON rm.ActionContextID = ac.ActionContextID " +
                    " INNER JOIN [Cashflow].[core].[ActionContextGroup] AS acg ON ac.ActionContextGroupID = acg.ActionContextGroupID " +
                    " WHERE acg.ActionContextGroupID = (SELECT TOP 1 acg1.ActionContextGroupID FROM [Cashflow].[log].[RawMessage] AS rm1 " +
                    " 															    INNER JOIN [Cashflow].[core].[ActionContext] AS ac1 ON rm1.ActionContextID = ac1.ActionContextID " +
                    " 								                  INNER JOIN [Cashflow].[core].[ActionContextGroup] AS acg1 ON ac1.ActionContextGroupID = acg1.ActionContextGroupID " +
                    " 																  WHERE " +
                    "                                   rm1.Created >= '" + data.From.ToString() + "' AND rm1.Created <= '" + data.To.ToString() + "' " +
                    " 																	AND rm1.RawMessageData LIKE '%" + data.SessionGuid + "%') " +
                    " ORDER BY rm.Created ASC ";
      }
      else
      {
        command = " SELECT " +
                    " rm.RawMessageID AS 'ID', " +
                    " rm.GroupKey AS 'Logger', " +
                    " rm.Thread, " +
                    " rm.RawMessageData AS 'Message', " +
                    " rm.Created AS 'Date' " +
                    " FROM Cashflow.core.IdentificationSession AS ids " +
                    " LEFT OUTER JOIN Cashflow.core.ActionContext AS ac ON ids.ActionContextID=ac.ActionContextID " +
                    " LEFT OUTER JOIN Cashflow.core.ActionContextGroup AS acg ON ac.ActionContextGroupID=acg.ActionContextGroupID " +
                    " LEFT OUTER JOIN Cashflow.core.ActionContext AS ac2 ON acg.ActionContextGroupID=ac2.ActionContextGroupID " +
                    " LEFT OUTER JOIN Cashflow.log.RawMessage AS rm ON rm.ActionContextID=ac2.ActionContextID " +
                    " WHERE " +
                    " ids.IdentificationSessionGuid='" + data.SessionGuid + "' " +
                    " AND rm.Created >= '" + data.From.ToString() + "' AND rm.Created <= '" + data.To.ToString() + "' " +
                    " ORDER BY rm.Created ASC; ";
      }
      #endregion

      this._result = new List<WebLogTable>();
      DirectContainer container = CashflowDirect.Instance.LoadContainer(command);
      this._result = container.ConvertList<WebLogTable>();
      return this._result;
    }



    public List<WebLogTable> QueryByReferenceGuid(WebLogTableData data)
    {
      string command = "";

      #region # sql command #
      command = " SELECT " +
                  " rm.RawMessageID AS 'ID', " +
                  " rm.GroupKey AS 'Logger', " +
                  " rm.Thread, " +
                  " rm.RawMessageData AS 'Message', " +
                  " rm.Created AS 'Date'" +
                  " FROM [Cashflow].[log].[RawMessage] AS rm " +
                  " INNER JOIN [Cashflow].[core].[ActionContext] AS ac ON rm.ActionContextID = ac.ActionContextID " +
                  " INNER JOIN [Cashflow].[core].[ActionContextGroup] AS acg ON ac.ActionContextGroupID = acg.ActionContextGroupID " +
                  " WHERE acg.ActionContextGroupID = (SELECT TOP 1 acg1.ActionContextGroupID FROM [Cashflow].[log].[RawMessage] AS rm1 " +
                  " 															    INNER JOIN [Cashflow].[core].[ActionContext] AS ac1 ON rm1.ActionContextID = ac1.ActionContextID " +
                  " 								                  INNER JOIN [Cashflow].[core].[ActionContextGroup] AS acg1 ON ac1.ActionContextGroupID = acg1.ActionContextGroupID " +
                  " 																  WHERE " +
                  "                                   rm1.Created >= '" + data.From.ToString() + "' AND rm1.Created <= '" + data.To.ToString() + "' " +
                  " 																	AND rm1.RawMessageData LIKE '%" + data.SessionID + "%') " +
                  " ORDER BY rm.Created ASC ";
      #endregion

      this._result = new List<WebLogTable>();
      DirectContainer container = CashflowDirect.Instance.LoadContainer(command);
      this._result = container.ConvertList<WebLogTable>();
      return this._result;
    }

    public List<WebLogTable> QueryByIdentificationSessionGuid(WebLogTableData data)
    {
      string command = "";

      #region # sql command #
      command = " SELECT " +
                    " rm.RawMessageID AS 'ID', " +
                    " rm.GroupKey AS 'Logger', " +
                    " rm.Thread, " +
                    " rm.RawMessageData AS 'Message', " +
                    " rm.Created AS 'Date' " +
                    " FROM Cashflow.core.IdentificationSession AS ids " +
                    " LEFT OUTER JOIN Cashflow.core.ActionContext AS ac ON ids.ActionContextID=ac.ActionContextID " +
                    " LEFT OUTER JOIN Cashflow.core.ActionContextGroup AS acg ON ac.ActionContextGroupID=acg.ActionContextGroupID " +
                    " LEFT OUTER JOIN Cashflow.core.ActionContext AS ac2 ON acg.ActionContextGroupID=ac2.ActionContextGroupID " +
                    " LEFT OUTER JOIN Cashflow.log.RawMessage AS rm ON rm.ActionContextID=ac2.ActionContextID " +
                    " WHERE " +
                    " ids.IdentificationSessionGuid='" + data.SessionID + "' AND rm.RawMessageID IS NOT NULL " +
                    " ORDER BY rm.Created ASC; ";
      #endregion

      this._result = new List<WebLogTable>();
      DirectContainer container = CashflowDirect.Instance.LoadContainer(command);
      this._result = container.ConvertList<WebLogTable>();
      return this._result;
    }

    public List<WebLogTable> QueryBySubscriptionRequestGuid(WebLogTableData data)
    {
      string command = "";

      #region # sql command #
      command = @"SELECT  rm.RawMessageID AS 'ID',  rm.GroupKey AS 'Logger',  rm.Thread,  rm.RawMessageData AS 'Message',  rm.Created AS 'Date'  
       FROM Cashflow.core.SubscriptionRequest AS sr
       LEFT OUTER JOIN Cashflow.core.ActionContext AS ac ON sr.ActionContextID=ac.ActionContextID  
       LEFT OUTER JOIN Cashflow.core.ActionContextGroup AS acg ON ac.ActionContextGroupID=acg.ActionContextGroupID  
       LEFT OUTER JOIN Cashflow.core.ActionContext AS ac2 ON acg.ActionContextGroupID=ac2.ActionContextGroupID  
       LEFT OUTER JOIN Cashflow.log.RawMessage AS rm ON rm.ActionContextID=ac2.ActionContextID  
       WHERE  sr.SubscriptionRequestGuid='"+ data.SessionID +"' AND rm.RawMessageID IS NOT NULL ORDER BY rm.Created ASC;";
      #endregion

      this._result = new List<WebLogTable>();
      DirectContainer container = CashflowDirect.Instance.LoadContainer(command);
      this._result = container.ConvertList<WebLogTable>();
      return this._result;
    }

    public List<WebLogTable> QueryByReferenceIntID(WebLogTableData data)
    {
      string command = "";

      #region # sql command #
      command = @"SELECT rm.RawMessageID AS 'ID',  rm.GroupKey AS 'Logger',  rm.Thread,  rm.RawMessageData AS 'Message',  rm.Created AS 'Date'  
        FROM Cashflow.core.ActionContextGroupReferenceMap AS map 
        LEFT OUTER JOIN Cashflow.core.ActionContext AS c ON map.ActionContextGroupID=c.ActionContextGroupID
        LEFT OUTER JOIN Cashflow.log.RawMessage AS rm ON rm.ActionContextID=c.ActionContextID
        WHERE map.ReferenceIntID=" + data.SessionID + "  AND rm.RawMessageID IS NOT NULL ;";
      #endregion

      this._result = new List<WebLogTable>();
      DirectContainer container = CashflowDirect.Instance.LoadContainer(command);
      this._result = container.ConvertList<WebLogTable>();
      return this._result;
    }




    // SUMMARY: Sql for getting all transactions for all paywall services
    public List<CashflowBehaviorModelTable> Query(EntranceTableData data, bool newTransactions = true)
    {
      string command = string.Empty;

      #region # sql command #

      char transactionModulator = newTransactions ? '=' : '>';
      command = " ( " +
                  " SELECT bm.Name, COUNT(*) FROM Cashflow.core.[Transaction] AS t " +
                  " LEFT OUTER JOIN Cashflow.core.TransactionGroup AS tg ON t.TransactionGroupID=tg.TransactionGroupID " +
                  " LEFT OUTER JOIN Cashflow.core.Subscription AS s ON s.TransactionGroupID=tg.TransactionGroupID " +
                  " LEFT OUTER JOIN Cashflow.core.SubscriptionRequest AS sr ON sr.SubscriptionRequestID=s.SubscriptionRequestID " +
                  " LEFT OUTER JOIN Cashflow.core.BehaviorModel AS bm ON bm.BehaviorModelID=sr.BehaviorModelID " +
                  " WHERE bm.ProductID=8 AND  " +
                    " t.Created >= '" + data.From + "' AND t.Created <= '"+ data.To +"'  AND t.TransactionStatusID=4 AND " +
                    " ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("bm.Name LIKE '%{0}%'", data.Services) + " ) AND "  +
	                  " ( SELECT COUNT(*) FROM Cashflow.core.[Transaction] WHERE TransactionGroupID=tg.TransactionGroupID) "+ transactionModulator +" 1 " +
                  " GROUP BY bm.Name " +
                  " ) " +
                  " UNION ALL " +
                  " ( " +
                  " SELECT bm.Name, COUNT(*) FROM Cashflow.core.[Transaction] AS t " +
                  " LEFT OUTER JOIN Cashflow.core.TransactionGroup AS tg ON t.TransactionGroupID=tg.TransactionGroupID " +
                  " LEFT OUTER JOIN Cashflow.core.Purchase AS s ON s.TransactionGroupID=tg.TransactionGroupID " +
                  " LEFT OUTER JOIN Cashflow.core.PurchaseRequest AS sr ON sr.PurchaseRequestID=s.PurchaseRequestID " +
                  " LEFT OUTER JOIN Cashflow.core.BehaviorModel AS bm ON bm.BehaviorModelID=sr.BehaviorModelID " +
                  " WHERE bm.ProductID=8 AND  " +
	                  " t.Created >= '"+ data.From +"' AND t.Created <= '"+ data.To +"' AND t.TransactionStatusID=4 AND " +
                    " ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("bm.Name LIKE '%{0}%'", data.Services) + " ) AND "  +
	                  " ( SELECT COUNT(*) FROM Cashflow.core.[Transaction] WHERE TransactionGroupID=tg.TransactionGroupID) "+ transactionModulator +" 1 " +
                  " GROUP BY bm.Name " +
                  " ) " +
                  " ORDER BY Name; ";

      #endregion

      CashflowDirect database = CashflowDirect.Instance;
      DataTable table = database.Load(command);
      if (table == null)
        return new List<CashflowBehaviorModelTable>();

      List<CashflowBehaviorModelTable> result = new List<CashflowBehaviorModelTable>();
      foreach(DataRow row in table.Rows)
        result.Add(new CashflowBehaviorModelTable(row));

      return result;
    }

  }
}
