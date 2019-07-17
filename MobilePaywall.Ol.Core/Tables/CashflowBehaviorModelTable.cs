using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{
  public class CashflowBehaviorModelTable
  {
    private string _name = string.Empty;
    private int _newTransactions = -1;
    private int _subsequentTransaction = -1;

    public string Name { get { return this._name; } }
    public string NewTransaction { get { return this._newTransactions.ToString(); } }
    public string SubsequentTransaction { get { return "(not implemented)"; } }

    public enum Columns
    {
      Name,
      NewTransactions
    }

    public CashflowBehaviorModelTable(DataRow row)
    {
      this._name = row[(int)CashflowBehaviorModelTable.Columns.Name].ToString();
      Int32.TryParse(row[(int)CashflowBehaviorModelTable.Columns.NewTransactions].ToString(), out this._newTransactions);
    }

  }
}
