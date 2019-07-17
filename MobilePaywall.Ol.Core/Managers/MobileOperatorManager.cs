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
  public class MobileOperatorManager : MobilePaywallDatabase
  {
    private List<MobileOperatorTable> _result = null;

    public List<MobileOperatorTable> Result { get { return this._result; } }

    public MobileOperatorManager() : base()
    {

    }

    public List<MobileOperatorTable> Query()
    {
      string command = "";
      #region # Sql command #

      command = " SELECT  " +
                " 	mo.MobileOperatorID, " +
                " 	mo.Name, " +
                " 	co.CountryID, " +
                " 	co.TLIC " +
                " FROM MobilePaywall.core.MobileOperator AS mo, " +
                " ( SELECT FallbackCountryID AS FCID FROM MobilePaywall.core.Service WHERE FallbackCountryID IS NOT NULL GROUP BY FallbackCountryID ) AS cmo, " +
                " ( SELECT CountryID, TwoLetterIsoCode AS TLIC FROM MobilePaywall.core.Country ) AS co " +
                " WHERE cmo.FCID=mo.CountryID AND cmo.FCID=co.CountryID AND mo.ExternalMobileOperatorID IS NOT NULL " +
                " ORDER BY mo.CountryID DESC; ";

      #endregion

      this._result = new List<MobileOperatorTable>();
      DataTable table = this.Load(command);
      if (table == null)
        return this._result;

      foreach(DataRow row in table.Rows)
      {
        MobileOperatorTable tableEntry = new MobileOperatorTable()
        {
          ID = Int32.Parse(row[(int)MobileOperatorColumns.ID].ToString()),
          Name = row[(int)MobileOperatorColumns.Name].ToString(),
          CountryID = Int32.Parse(row[(int)MobileOperatorColumns.CountryID].ToString()),
          Country = row[(int)MobileOperatorColumns.CountryName].ToString()
        };
        _result.Add(tableEntry);
      }

      return this._result;
    }

  }
}
