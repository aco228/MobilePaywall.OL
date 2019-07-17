using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePaywall.Ol.Core.Models
{
  public class DatabaseReportModel
  {
    public List<DatabaseReportActivityEntry> _entries = null;

    public List<DatabaseReportActivityEntry> Entries { get { return this._entries; } }

    public DatabaseReportModel(List<DatabaseReportActivityEntry> data)
    {
      this._entries = data;
    }
  }

  public class DatabaseReportActivityEntry
  {
    private DataRow _row = null;

    public string JobName { get { return this._row[(int)Columns.JobName].ToString(); } }
    public string Description { get { return this._row[(int)Columns.Description].ToString(); } }
    public string LastRunDate { get { return this._row[(int)Columns.LastRunDate].ToString(); } }
    public string LastRunStatus { get { return this._row[(int)Columns.LastRunStatus].ToString(); } }

    public DatabaseReportActivityEntry(DataRow row)
    {
      this._row = row;
    }

    public enum Columns
    {
      JobName,
      Description,
      LastRunDate,
      LastRunStatus
    }

  }
}
