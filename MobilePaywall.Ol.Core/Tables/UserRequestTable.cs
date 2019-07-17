using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{
  public class UserRequestTable
  {
    private string _created = "";
    private string _requestedUrl = "";

    public string Created { get { return this._created; } set { this._created = value; } }
    public string RequestedUrl { get { return this._requestedUrl; } set { this._requestedUrl = value; } }

  }

  public enum UserRequestColumns
  {
    RequestedUrl,
    Created
  }

}
