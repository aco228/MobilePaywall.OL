using MobilePaywall.Ol.Core.Data;
using MobilePaywall.Ol.Core.Managers;
using MobilePaywall.Ol.Core.Models;
using MobilePaywall.Ol.Core.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class AndroidController : Controller
  {
    // GET: Android
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult GetData()
    {
      AndroidInputModel inputModel = new AndroidInputModel(this.Request);
      EntranceTableData inputData = inputModel.ToEntranceTableData();
      if (inputData.Validation())
        return this.Json(new { status=false }, JsonRequestBehavior.AllowGet);

      EntranceTableManager entranceTableManager = new EntranceTableManager();
      List<EntranceTableAndroid> result = entranceTableManager.QueryNewAndroid(inputData);
      
      return this.Json(result, JsonRequestBehavior.AllowGet);

    }

  }


}