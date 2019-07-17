using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Helpers
{
  public class ViewDateConverter
  {
    public static string GetDate(string date)
    {
      DateTime temp;
      if (DateTime.TryParse(date, out temp))
        return ViewDateConverter.GetDate(temp);
      return string.Empty;
    }

    public static string GetDate(DateTime? date)
    {
      if (!date.HasValue)
        return string.Empty;

      bool sameYear = (date.Value.Year == DateTime.Now.Year),
        sameMonth = (date.Value.Month == DateTime.Now.Month),
        sameDay = (date.Value.Day == DateTime.Now.Day);

      // nothing is the same
      if (!sameYear && !sameMonth && !sameDay)
        return date.Value.ToString("MM/dd/yy H:mm:ss");

      // same year but not month && day
      if(sameYear && (!sameMonth && !sameDay))
        return date.Value.ToString("MM/dd H:mm:ss");

      if (sameYear && sameMonth && !sameDay)
        return date.Value.ToString("MM/dd H:mm:ss");

      if (sameYear && sameMonth && sameDay)
        return date.Value.ToString("H:mm:ss");

      return date.Value.ToString("MM/dd/yy H:mm:ss");
    }

    public static string PrintDate(string date)
    {
      DateTime temp;
      if (DateTime.TryParse(date, out temp))
        return ViewDateConverter.PrintDate(temp);
      return string.Empty;
    }
    
    public static string PrintDate(DateTime? date)
    {
      if (!date.HasValue)
        return string.Empty;
      return date.Value.ToString("MM/dd/yyyy H:mm:ss");
    }

  }
}
