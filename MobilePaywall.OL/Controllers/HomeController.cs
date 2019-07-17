using MobilePaywall.Ol.Core.Managers;
using MobilePaywall.Ol.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobilePaywall.Data;

namespace MobilePaywall.OL.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      return Redirect("/Database");
    }

  }
}