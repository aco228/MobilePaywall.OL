using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class ModelBase
  {
    private string _htmlTitle = string.Empty;

    public string HtmlTitle { get { return this._htmlTitle; } set { this._htmlTitle = value; } }
  }
}
