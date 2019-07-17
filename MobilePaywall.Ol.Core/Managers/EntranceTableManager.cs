using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MobilePaywall.Ol.Core.Database;
using MobilePaywall.Ol.Core.Tables;
using MobilePaywall.Ol.Core.Data;
using System.Data;
using MobilePaywall.Direct;

namespace MobilePaywall.Ol.Core.Managers
{
  public class EntranceTableManager
  {

    private List<EntranceTable> _result = null;
    private string _lastCommand = string.Empty;
    private MobilePaywallDirect _database = null;

    public List<EntranceTable> Result { get { return this._result; } }
    public int Count { get { return this._result != null ? this._result.Count : 0; } }
    public string LastCommand { get { return this._lastCommand; } }
    
    public EntranceTableManager()
    {
      this._database = new MobilePaywallDirect();
    }

    public List<EntranceTable> Query(EntranceTableData data)
    {
      string command = "";
      #region # sql command #

      #region # set Date reference for loading. Load by user session date or transaction date (with specific user session date ) #
      
      string timeLoad = string.Empty;
      if (data.UserSessionReference.Equals("false"))
      {
        timeLoad = "	AND t.Created > '" + data.From + "' AND t.Created < '" + data.To + "' AND p.Created <= DATEADD(day, -1, CONVERT(DATE, '" + data.From + "') ) ";
        if (data.TransactionUseUserSessionDate)
          timeLoad += string.Format(" AND us.Created >= '{0}' AND us.Created <= '{1}' ", data.TransactionUserSessionDateFrom, data.TransactionUserSessionDateTo);
      }
      else
        timeLoad = ((int)data.TransactionSearchType == 1 ? "AND t.Created > '" + data.From + "' AND t.Created < '" + data.To + "'" : "") + " AND us.Created > '" + data.From + "' AND us.Created < '" + data.To + "'  ";

      #endregion

      command = " SELECT " + Environment.NewLine
                + data.Top + Environment.NewLine
                + " us.UserSessionID, " + Environment.NewLine
                + " country.TwoLetterIsoCode, " + Environment.NewLine
                + " country.GlobalName, " + Environment.NewLine
                + " us.UserSessionGuid AS 'SGID'," + Environment.NewLine
                + " us.Created AS 'SCreated'," + Environment.NewLine
                + " us.EntranceUrl AS 'EntranceUrl', " + Environment.NewLine
                + " service.Name AS 'ServiceName'," + Environment.NewLine
                + " mo.Name AS 'Operator'," + Environment.NewLine
                + " ls.IdentificationSessionGuid, " + Environment.NewLine
                + " us.IPAddress AS 'IP'," + Environment.NewLine
                + " c.Msisdn," + Environment.NewLine
                + " pr.PaymentRequestID, " + Environment.NewLine
                + " prStatus.Name, " + Environment.NewLine
                + " pr.ExternalPaymentRequestGuid," + Environment.NewLine
                + " pr.PaymentRedirectUrl," + Environment.NewLine
                + " p.ExternalPaymentGuid," + Environment.NewLine
                + " paymentStatus.Name AS 'PStatus'," + Environment.NewLine
                + " p.Created AS 'PCreated'," + Environment.NewLine
                + " pcapm.PaymentContentAccessPolicyMapID, " + Environment.NewLine
                + " t.TransactionID AS 'TID'," + Environment.NewLine
                + " t.Created AS 'TCreated'" + Environment.NewLine
                + " FROM MobilePaywall.core.UserSession AS us " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.Country AS country ON us.CountryID = country.CountryID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.Service AS service ON us.ServiceID = service.ServiceID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.Customer AS c ON us.CustomerID=c.CustomerID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.MobileOperator AS mo ON c.MobileOperatorID=mo.MobileOperatorID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.LookupSession AS ls ON us.UserSessionID=ls.UserSessionID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.LookupSessionResult AS lsr ON ls.LookupSessionID=lsr.LookupSessionID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON us.UserSessionID=pr.UserSessionID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.PaymentRequestStatus AS prStatus ON pr.PaymentRequestStatusID=prStatus.PaymentRequestStatusID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON p.PaymentRequestID=pr.PaymentRequestID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.PaymentStatus AS paymentStatus ON p.PaymentStatusID = paymentStatus.PaymentStatusID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.PaymentContentAccessPolicyMap AS pcapm ON pcapm.PaymentID=p.PaymentID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.[Transaction] AS t ON pcapm.TransactionID=t.TransactionID  " + Environment.NewLine
                + " WHERE " + Environment.NewLine
                + " ( '" + data.Country + "' = '' " + DataObjectBase.PrepareList("service.Description LIKE UPPER('{0}') + '%' ", "service.Description NOT LIKE UPPER('{0}') + '%' ", data.Countries) + " )" + Environment.NewLine
                + " AND ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("service.Name LIKE '%{0}%'", "service.Name NOT LIKE '%{0}%'", data.Services) + " ) " + Environment.NewLine
                + " AND ( '" + data.MobileOperator + "' = -1 OR " + DataObjectBase.PrepareData("mo.MobileOperatorID", data.MobileOperator) + " )" + Environment.NewLine
                + " AND ( '" + (int)data.PaymentRequestSearchType + "' = -1 OR ( '" + (int)data.PaymentRequestSearchType + "' = 0 AND pr.PaymentRequestID IS NULL ) OR ( '" + (int)data.PaymentRequestSearchType + "' = 1 AND pr.PaymentRequestID IS NOT NULL ) )" + Environment.NewLine
                + " AND ( '" + (int)data.PaymentSearchType + "' = -1 OR ( '" + (int)data.PaymentSearchType + "' = 0 AND p.PaymentID IS NULL ) OR ( '" + (int)data.PaymentSearchType + "' = 1 AND p.PaymentID IS NOT NULL ) )" + Environment.NewLine
                + " AND ( '" + (int)data.TransactionSearchType + "' = -1 OR ( '" + (int)data.TransactionSearchType + "' = 0 AND t.TransactionID IS NULL ) OR ( '" + (int)data.TransactionSearchType + "' = 1 AND t.TransactionID IS NOT NULL ) ) " + Environment.NewLine
                + " AND ( '" + data.UseOLReference + "' = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) " + Environment.NewLine
                + " AND ( '" + data.Msisdn + "' = '' " + DataObjectBase.PrepareList("c.Msisdn='{0}'", data.Msisdns) + " " + DataObjectBase.PrepareList("c.EncryptedMsisdn = '{0}'", data.Msisdns) + " ) " + Environment.NewLine
                + " AND ( '" + data.Pxid + "' = '' " + DataObjectBase.PrepareList("us.EntranceUrl LIKE '%pxid={0}%'", data.Pxids) + " ) " + Environment.NewLine
                + " AND ( '" + data.IP + "' = '' " + DataObjectBase.PrepareList("us.IPAddress='{0}'", data.IPS) + " ) " + Environment.NewLine
                 //+ " AND ( '" + data.ReferrerContains + "'='' " + DataObjectBase.PrepareList("us.Referrer LIKE '%{0}%'", data.ReferrerContainsList) + " ) "  + Environment.NewLine
                + " AND ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) " + Environment.NewLine
                + " AND ( '" + data.PaymentStatus + "' = -1 OR ( p.PaymentStatusID = '" + data.PaymentStatus + "' ) ) " + Environment.NewLine
                + timeLoad + Environment.NewLine
                + " ORDER BY us.UserSessionID DESC";
      this._lastCommand = command;

      #endregion

      this._result = new List<EntranceTable>();
      this._database.SetTimeout(0);
      DataTable table = this._database.Load(command);
      if (table == null)
        return this._result; ;

      foreach(DataRow row in table.Rows)
      {
        EntranceTable tableRow = new EntranceTable(row);
        this._result.Add(tableRow);
      }

      return this._result;
    }

    public List<EntranceTableNew> QueryNew(EntranceTableData data)
    {
      string command = "";
      #region # sql command #

      #region # SPECIAL CASES #

      // referer if needed
      string _REFERER_JOIN = string.IsNullOrEmpty(data.ReferrerContains) ? "" : " LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.UserSessionID=cache.UserSessionID ";
      string _REFERER_WHERE = string.IsNullOrEmpty(data.ReferrerContains) ? "" : " AND ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) ";

      string _TIME = " AND cache.SessionCreated >= '" + data.From + "' AND cache.SessionCreated <= '" + data.To + "' " + Environment.NewLine;
      if (data.UserSessionReference.Equals("false"))
      {
        _TIME = " AND cache.Created >= '" + data.From + "' AND cache.Created <= '" + data.To + "' ";
        if (data.TransactionUseUserSessionDate)
          _TIME += " AND cache.SessionCreated >= '" + data.TransactionUserSessionDateFrom + "' AND cache.SessionCreated <= '" + data.TransactionUserSessionDateTo + "' ";
      }

      string _OLCACHE_SEQUENTAL = (string.IsNullOrEmpty(data.OLCacheID) ? "" : " AND cache.OLCacheID<" + data.OLCacheID);

      #endregion

      command = " SELECT  " + Environment.NewLine
                + data.Top + Environment.NewLine
                + " cache.OLCacheID, " + Environment.NewLine
                + " cache.UserSessionID, " + Environment.NewLine
                + " cache.ServiceID, " + Environment.NewLine
                + " cache.Pxid, " + Environment.NewLine
                + " cache.IPAddress, " + Environment.NewLine
                + " cache.ServiceName, " + Environment.NewLine
                + " cache.ServiceCountry, " + Environment.NewLine
                + " cache.CountryName, " + Environment.NewLine
                + " cache.CountryCode, " + Environment.NewLine
                + " cache.MobileOperatorID, " + Environment.NewLine
                + " cache.MobileOperatorName, " + Environment.NewLine
                + " cache.IdentificationSessionGuid, " + Environment.NewLine
                + " cache.Msisdn, " + Environment.NewLine
                + " cache.PaymentRequestID, " + Environment.NewLine
                + " pr.PaymentRequestStatusID, " + Environment.NewLine
                + " cache.PaymentID, " + Environment.NewLine
                + " cache.PaymentCreated, " + Environment.NewLine
                + " p.PaymentStatusID, " + Environment.NewLine
                + " cache.PaymentContentAccessPolicyID, " + Environment.NewLine
                + " cache.TransactionID, " + Environment.NewLine
                + " cache.TransactionCreated, " + Environment.NewLine
                + " cache.IsSubseguent, " + Environment.NewLine
                + " cache.SessionCreated " + Environment.NewLine
                + " FROM MobilePaywall.core.OLCache AS cache " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON cache.PaymentRequestID=pr.PaymentRequestID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON cache.PaymentID=p.PaymentID " + Environment.NewLine
                + _REFERER_JOIN + Environment.NewLine
                + " WHERE " + Environment.NewLine
                + " ( '" + data.Country + "' = '' " + DataObjectBase.PrepareList("cache.ServiceCountry = '{0}' ", "cache.ServiceCountry != '{0}' ", data.Countries) + " )" + Environment.NewLine
                + " AND ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("cache.ServiceName LIKE '%{0}%'", "cache.ServiceName NOT LIKE '%{0}%'", data.Services) + " ) " + Environment.NewLine
                + " AND ( '" + data.MobileOperator + "' = -1 OR " + DataObjectBase.PrepareData("cache.MobileOperatorID", data.MobileOperator) + " )" + Environment.NewLine
                + " AND ( '" + (int)data.PaymentRequestSearchType + "' = -1 OR ( '" + (int)data.PaymentRequestSearchType + "' = 0 AND cache.PaymentRequestID IS NULL ) OR ( '" + (int)data.PaymentRequestSearchType + "' = 1 AND cache.PaymentRequestID IS NOT NULL ) )" + Environment.NewLine
                + " AND ( '" + (int)data.PaymentSearchType + "' = -1 OR ( '" + (int)data.PaymentSearchType + "' = 0 AND cache.PaymentID IS NULL ) OR ( '" + (int)data.PaymentSearchType + "' = 1 AND cache.PaymentID IS NOT NULL ) )" + Environment.NewLine
                + " AND ( '" + (int)data.TransactionSearchType + "' = -1 OR ( '" + (int)data.TransactionSearchType + "' = 0 AND cache.TransactionID IS NULL ) OR ( '" + (int)data.TransactionSearchType + "' = 1 AND cache.TransactionID IS NOT NULL ) ) " + Environment.NewLine
                //+ " AND ( '" + data.UseOLReference + "' = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) " + Environment.NewLine
                + " AND ( '" + data.Msisdn + "' = '' " + DataObjectBase.PrepareList("cache.Msisdn='{0}'", data.Msisdns) + " ) " + Environment.NewLine
                + " AND ( '" + data.Pxid + "' = '' " + DataObjectBase.PrepareList("cache.Pxid LIKE '{0}'", data.Pxids) + " ) " + Environment.NewLine
                + " AND ( '" + data.IP + "' = '' " + DataObjectBase.PrepareList("cache.IPAddress='{0}'", data.IPS) + " ) " + Environment.NewLine
                //+ " AND ( '" + data.ReferrerContains + "'='' " + DataObjectBase.PrepareList("us.Referrer LIKE '%{0}%'", data.ReferrerContainsList) + " ) "  + Environment.NewLine
                //+ " AND ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) " + Environment.NewLine
                + " AND ( '" + data.PaymentStatus + "' = -1 OR ( p.PaymentStatusID = '" + data.PaymentStatus + "' ) ) " + Environment.NewLine
                + " AND " + (data.UserSessionReference.Equals("true") ? " cache.IsSubseguent=0 " : " cache.IsSubseguent=1 ") + Environment.NewLine
                + _REFERER_WHERE + Environment.NewLine
                + _TIME + Environment.NewLine
                + _OLCACHE_SEQUENTAL + Environment.NewLine
                + " ORDER BY cache.OLCacheID DESC";
      #endregion

      this._lastCommand = command;

      List<EntranceTableNew> result = new List<EntranceTableNew>();
      this._database.SetTimeout(0);
      DataTable table = this._database.Load(command);
      if (table == null)
        return result;

      foreach(DataRow row in table.Rows)
      {
        EntranceTableNew tableRow = new EntranceTableNew(row);
        result.Add(tableRow);
      }

      return result;
    }
    
    public List<EntranceTableNew> QueryNewAndroidSession(EntranceTableData data)
    {
      string command = "";
      #region # sql command #

      command = " SELECT  " + Environment.NewLine
                + data.Top + Environment.NewLine
                + " cache.UserSessionID, " + Environment.NewLine
                + " cache.ServiceID, " + Environment.NewLine
                + " cache.Pxid, " + Environment.NewLine
                + " cache.IPAddress, " + Environment.NewLine
                + " cache.ServiceName, " + Environment.NewLine
                + " cache.ServiceCountry, " + Environment.NewLine
                + " cache.CountryName, " + Environment.NewLine
                + " cache.CountryCode, " + Environment.NewLine
                + " cache.MobileOperatorID, " + Environment.NewLine
                + " cache.MobileOperatorName, " + Environment.NewLine
                + " cache.IdentificationSessionGuid, " + Environment.NewLine
                + " cache.Msisdn, " + Environment.NewLine
                + " cache.PaymentRequestID, " + Environment.NewLine
                + " pr.PaymentRequestStatusID, " + Environment.NewLine
                + " cache.PaymentID, " + Environment.NewLine
                + " cache.PaymentCreated, " + Environment.NewLine
                + " p.PaymentStatusID, " + Environment.NewLine
                + " cache.PaymentContentAccessPolicyID, " + Environment.NewLine
                + " cache.TransactionID, " + Environment.NewLine
                + " cache.TransactionCreated, " + Environment.NewLine
                + " cache.IsSubseguent, " + Environment.NewLine
                + " cache.SessionCreated " + Environment.NewLine
                + " FROM MobilePaywall.core.OLCache AS cache " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON cache.PaymentRequestID=pr.PaymentRequestID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON cache.PaymentID=p.PaymentID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.AndroidClientSessionOLCacheMap AS map ON map.OLCacheID=cache.OLCacheID "+ Environment.NewLine
                + " WHERE map.AndroidClientSessionID=" + data.AndroidClientSession.Value + Environment.NewLine 
                + " ORDER BY cache.OLCacheID DESC";

      #endregion

      this._lastCommand = command;

      List<EntranceTableNew> result = new List<EntranceTableNew>();
      this._database.SetTimeout(0);
      DataTable table = this._database.Load(command);
      if (table == null)
        return result;

      foreach (DataRow row in table.Rows)
      {
        EntranceTableNew tableRow = new EntranceTableNew(row);
        result.Add(tableRow);
      }

      return result;
    }

    public List<EntranceTableAndroid> QueryNewAndroid(EntranceTableData data)
    {
      string command = "";
      #region # sql command #

      #region # SPECIAL CASES #

      // referer if needed
      string _REFERER_JOIN = string.IsNullOrEmpty(data.ReferrerContains) ? "" : " LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON us.UserSessionID=cache.UserSessionID ";
      string _REFERER_WHERE = string.IsNullOrEmpty(data.ReferrerContains) ? "" : " AND ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) ";

      #endregion

      command = " SELECT  " + Environment.NewLine
                + data.Top + Environment.NewLine
                + " cache.UserSessionID, " + Environment.NewLine
                + " cache.ServiceName, " + Environment.NewLine
                + " cache.CountryCode, " + Environment.NewLine
                + " cache.MobileOperatorName, " + Environment.NewLine
                + " cache.IdentificationSessionGuid, " + Environment.NewLine
                + " cache.PaymentRequestID, " + Environment.NewLine
                + " p.PaymentStatusID, " + Environment.NewLine
                + " cache.TransactionID, " + Environment.NewLine
                + " cache.SessionCreated " + Environment.NewLine
                + " FROM MobilePaywall.core.OLCache AS cache " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.PaymentRequest AS pr ON cache.PaymentRequestID=pr.PaymentRequestID " + Environment.NewLine
                + " LEFT OUTER JOIN MobilePaywall.core.Payment AS p ON cache.PaymentID=p.PaymentID " + Environment.NewLine
                + _REFERER_JOIN + Environment.NewLine
                + " WHERE " + Environment.NewLine
                + " ( '" + data.Country + "' = '' " + DataObjectBase.PrepareList("cache.ServiceCountry = '{0}' ", "cache.ServiceCountry != '{0}' ", data.Countries) + " )" + Environment.NewLine
                + " AND ( '" + data.Service + "' = '' " + DataObjectBase.PrepareList("cache.ServiceName LIKE '%{0}%'", "cache.ServiceName NOT LIKE '%{0}%'", data.Services) + " ) " + Environment.NewLine
                + " AND ( '" + data.MobileOperator + "' = -1 OR " + DataObjectBase.PrepareData("cache.MobileOperatorID", data.MobileOperator) + " )" + Environment.NewLine
                + " AND ( '" + (int)data.PaymentRequestSearchType + "' = -1 OR ( '" + (int)data.PaymentRequestSearchType + "' = 0 AND cache.PaymentRequestID IS NULL ) OR ( '" + (int)data.PaymentRequestSearchType + "' = 1 AND cache.PaymentRequestID IS NOT NULL ) )" + Environment.NewLine
                + " AND ( '" + (int)data.PaymentSearchType + "' = -1 OR ( '" + (int)data.PaymentSearchType + "' = 0 AND cache.PaymentID IS NULL ) OR ( '" + (int)data.PaymentSearchType + "' = 1 AND cache.PaymentID IS NOT NULL ) )" + Environment.NewLine
                + " AND ( '" + (int)data.TransactionSearchType + "' = -1 OR ( '" + (int)data.TransactionSearchType + "' = 0 AND cache.TransactionID IS NULL ) OR ( '" + (int)data.TransactionSearchType + "' = 1 AND cache.TransactionID IS NOT NULL ) ) " + Environment.NewLine
                //+ " AND ( '" + data.UseOLReference + "' = 0 OR us.Referrer LIKE '" + data.OLRefferer + "' + '%' ) " + Environment.NewLine
                + " AND ( '" + data.Msisdn + "' = '' " + DataObjectBase.PrepareList("cache.Msisdn='{0}'", data.Msisdns) + " ) " + Environment.NewLine
                + " AND ( '" + data.Pxid + "' = '' " + DataObjectBase.PrepareList("cache.Pxid LIKE '{0}'", data.Pxids) + " ) " + Environment.NewLine
                + " AND ( '" + data.IP + "' = '' " + DataObjectBase.PrepareList("cache.IPAddress='{0}'", data.IPS) + " ) " + Environment.NewLine
                //+ " AND ( '" + data.ReferrerContains + "'='' " + DataObjectBase.PrepareList("us.Referrer LIKE '%{0}%'", data.ReferrerContainsList) + " ) "  + Environment.NewLine
                //+ " AND ( '" + data.ReferrerContains + "'='' " + data.PrepareReffererParts() + " ) " + Environment.NewLine
                + " AND ( '" + data.PaymentStatus + "' = -1 OR ( p.PaymentStatusID = '" + data.PaymentStatus + "' ) ) " + Environment.NewLine
                + " AND " + (data.TransactionUseUserSessionDate ? " cache.IsSubseguent=1 " : " cache.IsSubseguent=0 ") + Environment.NewLine
                + _REFERER_WHERE + Environment.NewLine
                + " AND cache.SessionCreated >= '" + data.From + "' AND cache.SessionCreated <= '" + data.To + "' " + Environment.NewLine
                + " ORDER BY cache.OLCacheID DESC";

      #endregion

      this._lastCommand = command;

      List<EntranceTableAndroid> result = new List<EntranceTableAndroid>();
      this._database.SetTimeout(0);
      DataTable table = this._database.Load(command);
      if (table == null)
        return result;

      foreach (DataRow row in table.Rows)
      {
        EntranceTableAndroid tableRow = new EntranceTableAndroid(row);
        result.Add(tableRow);
      }

      return result;
    }

    public string ConvertToCvs()
    {
      string data = "CountryIsoCode; CountryName; UserSessionGuid; UserSessionCreated; Pxid; EntranceUrl; ServiceName; MobileOperator; IdentificationSessionGuid; IPAddress; Msisdn; PaymentRequestID; PaymentReqeustStatus; ExternalPaymentRequestGuid; PaymentRedirectUrl; ExternalPaymentGuid; PaymentStatus; PaymentCreated; PaymentContentAccessPolicyMapID; TransactionID; TransactionCreated;" + Environment.NewLine;
      foreach (EntranceTable row in this._result)
        data += row.CsvRow + Environment.NewLine;
      return data;
    }

  }
}
