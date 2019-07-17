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
  public class UserRequestManager
  {

    private List<UserRequestTable> _result;

    public List<UserRequestTable> Result { get { return this._result; } }
    
    public UserRequestManager() { }

    public List<UserRequestTable> Query(WebLogTableData data)
    {
      string command = "";
      #region # command #
      command = " SELECT " +
              " 	uhr.RequestedUrl, " + 
              "   uhr.Created " + 
              " FROM  " + 
              " MobilePaywall.core.UserHttpRequest AS uhr " + 
              " LEFT OUTER JOIN MobilePaywall.core.UserSession AS us ON uhr.UserSessionID=us.UserSessionID " + 
              " WHERE us.UserSessionID='"+ data.SessionID +"' " +
              " ORDER BY uhr.Created DESC; ";
      #endregion

      DirectContainer container = MobilePaywallDirect.Instance.LoadContainer(command);
      this._result = container.ConvertList<UserRequestTable>();
      return this._result;
    }

    public List<List<string[]>> ConvertToLogModelInfomations()
    {
      List<string[]> data = new List<string[]>();
      List<List<string[]>> returnData = new List<List<string[]>>();
      if (this._result == null || this._result.Count == 0)
        return returnData;

      foreach (UserRequestTable row in this._result)
        data.Add(new string[] { row.Created, row.RequestedUrl });

      returnData.Add(data);
      return returnData;
    }

  }
}
