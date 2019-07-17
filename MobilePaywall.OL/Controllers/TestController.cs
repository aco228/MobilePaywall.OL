using MobilePaywall.Data;
using MobilePaywall.Data.Direct.MobilePaywall;
using MobilePaywall.Direct;
using Senti.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class TestController : Controller
  {
    public static string Status = "stop";
    public static string Message = "0";

    public static string Logs = string.Empty;

    public string Index()
    {
      string newStatus = Request["status"];
      if (!string.IsNullOrEmpty(newStatus))
        TestController.Status = newStatus;

      return TestController.Status;
    }

    public string Msg()
    {
      string newStatus = Request["start"];
      if (!string.IsNullOrEmpty(newStatus))
        TestController.Message = newStatus;

      return TestController.Message;
    }

    public string Log()
    {
      string newStatus = Request["text"];
      if (!string.IsNullOrEmpty(newStatus))
        TestController.Logs += DateTime.Now + " ---  " + newStatus + "<br/>";

      return TestController.Logs;
    }
    
    public DataObject t() { return Service.CreateManager().Load(1); }

  }


}