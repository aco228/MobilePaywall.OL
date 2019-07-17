using MobilePaywall.Data;
using MobilePaywall.Ol.Core;
using MobilePaywall.Ol.Core.Filters;
using MobilePaywall.Ol.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.OL.Controllers
{
  public class LoginController : PaywallController
  {
    public ActionResult Index()
    {
      LoginModel model = new LoginModel() { OriginalRequest = "/" };
      return View("~/Views/Login/Login.cshtml", model);
    }

    [LoginFilter(Required=false)]
    public ActionResult Login(string username, string password)
    {
      if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        return null;

      IClientManager cManager = Client.CreateManager();
      Client client = cManager.Load(username, password);

      if (client == null)
        return this.Json(new { status = false, message = "Wrong credentials", redirect = string.Empty });
      else if (client.ClientStatus == ClientStatus.Inactive)
        return this.Json(new { status = false, message = "Account is inactive", redirect = string.Empty });
      else
      {
        HttpCookie uid = new HttpCookie("uid");
        uid.Value = client.Username;
        uid.Expires = DateTime.Now.AddYears(5);
        Response.Cookies.Add(uid);

        HttpCookie pid = new HttpCookie("pid");
        pid.Value = this.Session.SessionID;
        pid.Expires = DateTime.Now.AddHours(12);
        Response.Cookies.Add(pid);

        return this.Json(new { status = true, message = "Success", redirect = "/" });
      }
    }

    public ActionResult Logout()
    {
      HttpCookie pid = this.Request.Cookies["pid"];
      if (pid != null)
      {
        pid.Expires = DateTime.Now.AddHours(-1);
        this.Response.Cookies.Add(pid);
      }
      return this.Redirect("/");
    }

  }
}