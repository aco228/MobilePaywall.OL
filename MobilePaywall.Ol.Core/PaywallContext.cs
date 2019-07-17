using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MobilePaywall.Ol.Core
{
  public class PaywallContext
  {

    protected static readonly string PaywallHttpContextItemKey = "PaywallHttpContextItemKey";
    protected static readonly string PaywallLocalizationItemKey = "PaywallLocalizationItemKey";

    public static PaywallContext Current
    {
      get
      {
        HttpContext httpContext = HttpContext.Current;
        return PaywallContext.GetCurrent(httpContext);
      }
    }

    public static PaywallContext GetCurrent(HttpContext httpContext)
    {
      lock (httpContext.Request)
      {
        PaywallContext paywallContext = httpContext.Items[PaywallHttpContextItemKey] as PaywallContext;
        if (paywallContext != null)
          return paywallContext as PaywallContext;

        paywallContext = new PaywallContext(httpContext);
        httpContext.Items[PaywallHttpContextItemKey] = paywallContext;
        return paywallContext;
      }
    }

    public PaywallContext(HttpContext context)
    {
      this.InitializeSession(context);
    }

    protected virtual void InitializeSession(HttpContext httpContext)
    {
      //INFO: DO NOT DO ANYTHING WITH THIS LINE BELOW!!!
      httpContext.Session["someValue"] = "bla";
    }
  }
}
