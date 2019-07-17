using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Database
{
  public abstract class DataObjectBase
  {
    protected int _selectTop = 0;
    protected bool _hasError = false;
    protected string _errorMessage = string.Empty;

    public int SelectTop { get { return this._selectTop; } set { this._selectTop = value; } }
    public string Top { get { return this._selectTop == 0 ? " TOP 1000 " : string.Format(" TOP {0} ", this._selectTop); } }
    public bool HasError { get { return this._hasError; } }
    public string ErrorMessage { get { return this._errorMessage; } }

    public DataObjectBase()
    { }

    public abstract bool Validation();

    public string ConvertDate(DateTime input)
    {
      return input.ToString("yyyy-MM-dd HH:mm:ss");
    }

    protected EntranceSearchType ConvertToSerachType(string input)
    {
      switch (input)
      {
        case "-1": return EntranceSearchType.Default; break;
        case "0": return EntranceSearchType.NULL; break;
        case "1": return EntranceSearchType.NOT_NULL; break;
        default: return EntranceSearchType.Default; break;
      }
    }

    // SUMMARY: this method returs multiple choices based of list
    //          pattern could be: "service.Name LIKE %'{0}'%"
    //          so data will be "OR (  service.Name LIKE %'porntraum'%  OR  service.Name LIKE %'geilevid'%  OR  service.Name LIKE %'tra'%  )"
    public static string PrepareList(string pattern, List<string> list)
    {
      if (list == null || list.Count == 0)
        return " OR " + string.Format(pattern, "");
        //return string.Empty;

      string data = " OR ( ";
      for (int i = 0; i < list.Count; i++ )
      {
        string listData = list.ElementAt(i);
        if(i > 0) data += " OR ";
        data += " " + string.Format(pattern, listData) + " ";
      }
      data += " ) ";
      return data;
    }

    public static string PrepareList(string positivePattern, string negativePattern, List<string> list)
    {
      if (list == null || list.Count == 0)
        return " OR " + string.Format(positivePattern, "");
      //return string.Empty;

      string data = " OR ( ";
      for (int i = 0; i < list.Count; i++)
      {
        string listData = list.ElementAt(i);
        if (i > 0) data += " OR ";

        bool negation = false;
        if (listData[0] == '!')
        {
          negation = true;
          listData = listData.Replace("!", string.Empty);
        }

        if (negation) 
          data += " " + string.Format(negativePattern, listData) + " ";
        else 
          data += " " + string.Format(positivePattern, listData) + " ";
        
      }
      data += " ) ";
      return data;
    }

    public static string PrepareData(string pattern, string data)
    {
      data = data.Trim();
      bool negation = false;
      if(data[0].Equals("!"))
      {
        negation = true;
        data = data.Replace("!", string.Empty);
      }

      if (negation)
        return string.Format("{0}!={1}", pattern, data);
      else
        return string.Format("{0}={1}", pattern, data);
    }


  }

  public enum EntranceSearchType
  {
    Default = -1,
    NULL = 0,
    NOT_NULL = 1
  }
  
}
