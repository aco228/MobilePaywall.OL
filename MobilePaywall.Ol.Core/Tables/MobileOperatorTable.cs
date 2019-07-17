using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Tables
{
  public class MobileOperatorTable
  {
    private int _id = -1;
    private string _name = string.Empty;
    private int _countryID = -1;
    private string _country = string.Empty;

    public int ID { get { return this._id; } set { this._id = value; } }
    public string Name { get { return this._name; } set { this._name = value; } }
    public int CountryID { get { return this._countryID; } set { this._countryID = value; } }
    public string Country { get { return this._country; } set { this._country = value;} }
  }

  public enum MobileOperatorColumns
  {
    ID,
    Name,
    CountryID,
    CountryName
  }
}
