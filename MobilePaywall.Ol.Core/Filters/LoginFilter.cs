using MobilePaywall.Ol.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MobilePaywall.Ol.Core.Filters
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public class LoginFilter : FilterBase
  {
    public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
    {
      if (!this.Required)
        return;

      HttpCookie uid = filterContext.RequestContext.HttpContext.Request.Cookies["uid"];
      HttpCookie pid = filterContext.RequestContext.HttpContext.Request.Cookies["pid"];

      if (uid == null || string.IsNullOrEmpty(uid.Value.ToString()))
      {
        this.Login(filterContext, string.Empty);
        return;
      }

      if(pid == null || string.IsNullOrEmpty(pid.Value.ToString()))
      {
        this.Login(filterContext, uid.Value.ToString());
        return;
      }

      //if (!pid.Value.ToString().Equals(PaywallContext.Current.))
      //{
      //  this.Login(filterContext, uid.Value.ToString());
      //  return;
      //}
      
      base.OnActionExecuting(filterContext);
    }

    private void Login(ActionExecutingContext filterContext, string username)
    {
      LoginModel model = new LoginModel() 
      { 
        Username = username ,
        OriginalRequest = filterContext.HttpContext.Request.RawUrl
      };

      ViewResult viewResult = new ViewResult();
      viewResult.ViewName = "~/Views/Login/Login.cshtml";
      viewResult.ViewData.Model = model;
      filterContext.Result = viewResult;
    }
  }
}
