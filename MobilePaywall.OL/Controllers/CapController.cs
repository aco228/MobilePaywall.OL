using MobilePaywall.Data;
using MobilePaywall.Ol.Core.Models;
using MobilePaywall.OL.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class CapController : Controller
  {
    // GET: Cap
    public ActionResult Index()
    {
      return View("~/Views/Cap/Index.cshtml", new TemplateServiceCapModel());
    }

    public ActionResult Call()
    {
      string id = Request["id"] != null ? Request["id"].ToString() : string.Empty;

      if (string.IsNullOrEmpty(id))
        return this.Content("nok");
      
      int serviceID;
      if (!Int32.TryParse(id, out serviceID))
        return this.Content("nok");

      Service service = Service.CreateManager().Load(serviceID);
      if (service == null)
        return this.Content("nok");

      List<TemplateServiceCap> caps = MobilePaywall.Web.PaywallCapManager.GetAllCaps(service);
      if(caps != null || caps.Count > 0)
        foreach(TemplateServiceCap cap in caps)
          ServiceCapHub.Current.Update(cap);

      ServiceCapHub.Current.Update(service);
      return this.Content("ok");
    }

  }
}