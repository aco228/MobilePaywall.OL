using MobilePaywall.Ol.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class LiveController : Controller
  {
    // GET: Live
    public ActionResult Index()
    {
      LiveModel model = new LiveModel();
      return View(model);
    }
  }
}