using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBBookmarks
{
    public class BookmarksDO
    {
        public BookmarksDO() { }

        public int Id { get; set; }
        public int LandBankId { get; set; }
        public string Interest { get; set; }
        public double UpperLimit { get; set; }
        public string? CompLimit { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public BookmarksDO(DataRow row)
        {
            Id = row.Table.Columns.Contains("Id") ? row["Id"].ToSafeInt() : 0;
            LandBankId = row.Table.Columns.Contains("LandBankId") ? row["LandBankId"].ToSafeInt() : 0;
            Interest = row.Table.Columns.Contains("Interest") ? row["Interest"].ToSafeString() : "";
            UpperLimit = row.Table.Columns.Contains("UpperLimit") ? row["UpperLimit"].ToSafeDouble() : 0;
            CompLimit = row.Table.Columns.Contains("CompLimit") ? row["CompLimit"].ToSafeString() : null;
            Notes = row.Table.Columns.Contains("Notes") ? row["Notes"].ToSafeString() : null;
            CreatedDate = row.Table.Columns.Contains("CreatedDate") ? row["CreatedDate"].ToSafeMinDate() : DateTime.MinValue;
            CreatedBy = row.Table.Columns.Contains("CreatedBy") ? row["CreatedBy"].ToSafeString() : null;
            UpdatedDate = row.Table.Columns.Contains("UpdatedDate") ? row["UpdatedDate"].ToSafeMinDate() : DateTime.MinValue;
            UpdatedBy = row.Table.Columns.Contains("UpdatedBy") ? row["UpdatedBy"].ToSafeString() : null;
        }
    }
}
