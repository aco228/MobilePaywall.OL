using MobilePaywall.Direct;
using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Managers
{
  public class CallbackLogManager
  {

    public List<WebLogTable> Query(string sessionID)
    {
      DirectDatabaseMsSql database = MobilePaywallLogDirect.Instance;

      // first we check if data is stored in log database
      int? check = database.LoadInt(string.Format(@"
        SELECT COUNT(*) FROM MobilePaywall.log.CallbackLog AS log
        LEFT OUTER JOIN MobilePaywall.log.CallbackLogUserSessionMap AS map ON map.CallbackLogID=log.CallbackLogID
        WHERE map.UserSessionID={0}", sessionID));
      
      if (!check.HasValue || check.Value == 0)
        database = MobilePaywallBackupDirect.Instance;

      DirectContainer container = database.LoadContainer(@"SELECT c.CallbackLogID AS 'ID', c.Date, c.Logger, c.Message, c.Method, c.Line
          FROM MobilePaywall.log.CallbackLogUserSessionMap AS map
          LEFT OUTER JOIN MobilePaywall.log.CallbackLog AS c ON map.CallbackLogID=c.CallbackLogID
          WHERE map.UserSessionID=" + sessionID + ";");
      return container.ConvertList<WebLogTable>();
    }

  }
}
