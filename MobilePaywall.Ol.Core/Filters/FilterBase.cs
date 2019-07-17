using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MobilePaywall.Ol.Core.Filters
{
  public abstract class FilterBase : ActionFilterAttribute
  {
    private bool _required = false;

    public bool Required { get { return this._required; } set { this._required = value; } }
  }
}
