using Microsoft.AspNet.SignalR;
using MobilePaywall.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.OL.Hubs
{
  public class ServiceCapHub : Hub
  {
    public static ServiceCapHub Current
    {
      get
      {
        return new ServiceCapHub(GlobalHost.ConnectionManager.GetHubContext<ServiceCapHub>());
      }
    }

    private IHubContext _context = null;

    public ServiceCapHub(IHubContext context)
    {
      this._context = context;
    }

    public void Update(TemplateServiceCap cap)
    {
      TimeSpan difference = DateTime.Now - cap.LastTransactionDate;

      var data = new
      {
        type = "cap", 
        serviceID = cap.Service.ID,
        serviceName = cap.Service.Name,
        serviceDescription = cap.Service.Description,
        country = cap.Service.FallbackCountry.TwoLetterIsoCode,
        capID = cap.ID,
        current = cap.TempCurrentValue,
        value = cap.Value,
        hourCreated = cap.LastTransactionDate.Hour,
        minuteCreated = cap.LastTransactionDate.Minute,
        secondCreated = cap.LastTransactionDate.Second,
        hour = (difference.Hours < 10 ?  "0" + difference.Hours : difference.Hours.ToString()),
        minute = (difference.Minutes < 10 ?  "0" + difference.Minutes : difference.Minutes.ToString()),
        second = (difference.Seconds < 10 ?  "0" + difference.Seconds : difference.Seconds.ToString())
      };

      if (this._context != null)
        this._context.Clients.All.update(data);
      else
        this.Clients.All.update(data);
    }

    public void Update(Service service)
    {
      var data = new
      {
        type = "service",
        serviceID = service.ID,
        serviceName = service.Name,
        serviceDescription = service.Description,
        country =  service.FallbackCountry.TwoLetterIsoCode,
        capID = -1,
        current = -1,
        value = -1,
        hourCreated = DateTime.Now.Hour,
        minuteCreated = DateTime.Now.Minute,
        secondCreated = -1,
        hour = -1,
        minute = -1,
        second = -1
      };

      if (this._context != null)
        this._context.Clients.All.update(data);
      else
        this.Clients.All.update(data);
    }
  }
}
