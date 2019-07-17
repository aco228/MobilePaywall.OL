using MobilePaywall.Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class InformationsBaseTableModel
  {
    private string _id = string.Empty;
    private string _tableName = string.Empty;
    private DirectContainer _container = null;
    private List<InformationsBaseTableModelEditable> _list = null;

    public string ID { get { return this._id; } set { this._id = value; } }
    public List<InformationsBaseTableModelEditable> List { get { return this._list; } }
    public DirectContainer Container { get { return this._container; } }
    public string TableName { get { return this._tableName; } set { this._tableName = value; } }

    public InformationsBaseTableModel(DirectContainer container)
    {
      this._container = container;
    }

  }

  public class InformationsBaseTableModelEditable
  {
    public enum EditableType { Unknown, String, Int, Checkbox, Select }
    private string _columnName = string.Empty;
    private EditableType _editableType = EditableType.Unknown;
    
    public EditableType EditableValueType { get { return this._editableType; } set { this._editableType = value; } }
    public string ColumnName { get { return this._columnName; } set { this._columnName = value; } }

    public InformationsBaseTableModelEditable(string columnName, EditableType type)
    {
      this._columnName = columnName;
      this._editableType = type;
    }
  }

}
