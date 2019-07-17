using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Database
{
  public class CashflowDatabase : DatabaseBase
  {
    public CashflowDatabase()
      : base("Cashflow", "core")
    {
      this.SetConnectionString("Data Source=192.168.11.100;Initial Catalog=Cashflow;uid=saCashflow;pwd=nm42-a>f.12GVc#1;");
    }
  }
}
