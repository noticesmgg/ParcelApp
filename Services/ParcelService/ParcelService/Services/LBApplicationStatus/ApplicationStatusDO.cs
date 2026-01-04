using DocumentFormat.OpenXml.Spreadsheet;
using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBApplicationStatus
{
    public class ApplicationStatusDO
    {
        public int Id { get; set; }
        public int LandBankId { get; set; }
        public string? AccountId { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ReSubmitDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public string? ApplicationNumber { get; set; }
        public string Status { get; set; }
        public double OurBid { get; set; }
        public string Competitor { get; set; }
        public double WinningBid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public ApplicationStatusDO()
        {
        }

        public ApplicationStatusDO(DataRow row)
        {
            Id = row.Table.Columns.Contains("Id") ? row["Id"].ToSafeInt() : 0;
            LandBankId = row.Table.Columns.Contains("LandBankId") ? row["LandBankId"].ToSafeInt() : 0;
            AccountId = row.Table.Columns.Contains("AccountId") ? row["AccountId"].ToSafeString() : null;
            SubmitDate = row.Table.Columns.Contains("SubmitDate") ? row["SubmitDate"].ToSafeMinNullDate() : null;
            ReSubmitDate = row.Table.Columns.Contains("ReSubmitDate") ? row["ReSubmitDate"].ToSafeMinNullDate() : null;
            AcceptedDate = row.Table.Columns.Contains("AcceptedDate") ? row["AcceptedDate"].ToSafeMinNullDate() : null;
            ApplicationNumber = row.Table.Columns.Contains("ApplicationNumber") ? row["ApplicationNumber"].ToSafeString() : null;
            Status = row.Table.Columns.Contains("StatusCode") ? row["StatusCode"].ToSafeString() : "";
            OurBid = row.Table.Columns.Contains("OurBid") ? row["OurBid"].ToSafeDouble() : 0;
            Competitor = row.Table.Columns.Contains("Competitor") ? row["Competitor"].ToSafeString() : null;
            WinningBid = row.Table.Columns.Contains("WinningBid") ? row["WinningBid"].ToSafeDouble() : 0;
            CreatedDate = row.Table.Columns.Contains("CreatedDate") ? row["CreatedDate"].ToSafeMinDate()  : DateTime.MinValue;
            UpdatedDate = row.Table.Columns.Contains("UpdatedDate") ? row["UpdatedDate"].ToSafeMinDate() : DateTime.MinValue;
            CreatedBy = row.Table.Columns.Contains("CreatedBy") ? row["CreatedBy"].ToSafeString() : null;
            UpdatedBy = row.Table.Columns.Contains("UpdatedBy") ? row["UpdatedBy"].ToSafeString() : null;
        }
    }
}
