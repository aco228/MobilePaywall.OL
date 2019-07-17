using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class ErrorModel : ModelBase
  {
    private string _title = "Error";
    private string _desciription = string.Empty;

    public string Title { get { return this._title; } set { this._title = value; } }
    public string Description { get { return this._desciription; } set { this._desciription = value; } }
  }
}
