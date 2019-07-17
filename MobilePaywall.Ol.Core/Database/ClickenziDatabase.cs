using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Database
{
  public class ClickenziDatabase : MysqlDatabaseBase
  {
    public ClickenziDatabase()
      : base("Clickenzi")
    {
      this.SetConnectionString("Server=188.214.128.107; database=Clickenzi; UID=saClickenzi; password=z8dg5jsx");
    }
  }
}
