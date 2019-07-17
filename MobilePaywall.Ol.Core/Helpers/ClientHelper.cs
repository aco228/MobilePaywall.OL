using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MobilePaywall.Ol.Core.Helpers
{
  public class ClientHelper
  {

    public static Client GetClient(HttpRequest request)
    {

      HttpCookie uid = request.Cookies["uid"];
      HttpCookie pid = request.Cookies["pid"];

      if (uid == null || pid == null)
        return null;

      Client clinet = Client.CreateManager().Load(uid.ToString());
      if (clinet == null)
        return null;

      return clinet;
    }

  }
}
