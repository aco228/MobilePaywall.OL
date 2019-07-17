using MobilePaywall.Data;
using MobilePaywall.Direct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class TemplateServiceCapModel
  {
    private List<Service> _services = null;
    private List<Country> _countries = null;

    public List<Service> Services { get { return this._services; } }
    public List<Country> Countries { get { return this._countries; } }

    public TemplateServiceCapModel()
    {

      MobilePaywallDirect db = MobilePaywallDirect.Instance;
      List<int> sids = db.LoadArrayInt(string.Format(@"
        SELECT s.ServiceID, s.FallbackCountryID
        FROM MobilePaywall.core.Service AS s
        INNER JOIN (SELECT ServiceID FROM MobilePaywall.core.TemplateServiceCap WHERE IsActive=1 GROUP BY ServiceID) AS m ON m.ServiceID=s.ServiceID
        ORDER BY s.FallbackCountryID"));

      IServiceManager sManager = Service.CreateManager();
      this._services = new List<Service>();
      this._countries = new List<Country>();
      foreach (int sid in sids)
      {
        Service s = Service.CreateManager().Load(sid);
        if (!this._countries.Contains(s.FallbackCountry))
          this._countries.Add(s.FallbackCountry);
        this._services.Add(s);
      }

    }

  }
  


}
