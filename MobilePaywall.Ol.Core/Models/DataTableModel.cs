using MobilePaywall.Ol.Core.Managers;
using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class DataTableModel : ModelBase
  {
    private List<EntranceTableNew> _newEntranceTableData = null;
    private EntranceTableManager _tableManager = null;

    public int Count { get { return this._tableManager != null ? this._tableManager.Count : 0; } }
    public string Command { get { return this._tableManager == null ? "" : this._tableManager.LastCommand; } }

    public List<EntranceTable> Data { get { return this._tableManager != null ? this._tableManager.Result : new List<EntranceTable>(); } }
    public List<EntranceTableNew> NewData { get { return this._newEntranceTableData; } }

    public DataTableModel(EntranceTableManager tableManager, List<EntranceTableNew> data = null)
    {
      this._tableManager = tableManager;
      this._newEntranceTableData = data;
    }
    
  }
}
