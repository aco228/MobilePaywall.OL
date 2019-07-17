using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class LogModel : ModelBase
  {
    private List<WebLogTable> _tables = null;
    private List<List<string[]>> _informations = null;
    private string _informationLabel = string.Empty;
    public List<WebLogTable> Tables { get { return this._tables; } set { this._tables = value; } }
    public List<List<string[]>> Informations { get { return this._informations; } set { this._informations = value; } }
    public string InformationsLabel { get { return this._informationLabel; } set { this._informationLabel = value; } }
    public int Count { get { return this._tables == null ? 0 : this._tables.Count; } }



  }
}
