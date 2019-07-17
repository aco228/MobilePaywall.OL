using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class LoginModel : ModelBase
  {
    private string _username = string.Empty;
    private string _originalRequest = string.Empty;
    private List<LoginServiceModel> _services = null;

    public string Username { get { return this._username; } set { this._username = value; } }
    public string OriginalRequest { get { return this._originalRequest; } set { this._originalRequest = value; } }
    public List<LoginServiceModel> Services { get { return this._services; } }

    public LoginModel()
    {
      this._services = new List<LoginServiceModel>();
      List<Service> services = Service.CreateManager().Load();
      foreach(Service s in services)
        this._services.Add(new LoginServiceModel(s));
    }
  }

  public class LoginServiceModel
  {
    private string _url = string.Empty;
    private string _id = string.Empty;

    public string Url
    {
      get { return string.Format("http://{0}/logo", this._url, this._id); }
    }

    public LoginServiceModel(Service service)
    {
      this._url = service.Name;
      this._id = service.ID.ToString();
    }
  }

}
