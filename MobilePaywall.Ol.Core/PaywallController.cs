using MobilePaywall.Ol.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MobilePaywall.Ol.Core
{
  [LoginFilter(Required=true)]
  public class PaywallController : Controller
  {

  }
}
