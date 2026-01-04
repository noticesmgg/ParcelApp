using DocumentFormat.OpenXml.Spreadsheet;
using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LandBank
{
    public class LandBankDO
    {
        public int? Id { get; set; }
        public string ParcelNumber { get; set; }
        public string ShortParcel { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? ZipCode { get; set; }
        public double? AskingPrice { get; set; }
        public double? UpdatedAskingPrice { get; set; }
        public DateTime? AdDate { get; set; }
        public DateTime? BidOffDate { get; set; }
        public DateTime? LastDateToApply { get; set; }
        public double? Acreage { get; set; }
        public int? SquareFoot { get; set; }
        public string Dimensions { get; set; }
        public bool? HasDemo { get; set; }
        public string PermitStatus { get; set; }
        public string PropertyStatus { get; set; }
        public string PropertyClassification { get; set; }
        public string Source { get; set; }
        public string Owner { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        public LandBankDO()
        { }

        public LandBankDO(DataRow row)
        {
            Id = row.Table.Columns.Contains("Id") ? row["Id"].ToSafeInt() : null;
            ParcelNumber = row.Table.Columns.Contains("ParcelNumber") ? row["ParcelNumber"].ToSafeString() : null;
            ShortParcel = row.Table.Columns.Contains("ShortParcel") ? row["ShortParcel"].ToSafeString() : null;
            Street = row.Table.Columns.Contains("Street") ? row["Street"].ToSafeString() : null;
            City = row.Table.Columns.Contains("City") ? row["City"].ToSafeString() : null;
            State = row.Table.Columns.Contains("State") ? row["State"].ToSafeString() : null;
            ZipCode = row.Table.Columns.Contains("ZipCode") ? row["ZipCode"].ToSafeInt() : null;
            AskingPrice = row.Table.Columns.Contains("AskingPrice") ? row["AskingPrice"].ToSafeDouble() : null;
            UpdatedAskingPrice = row.Table.Columns.Contains("UpdatedAskingPrice") ? row["UpdatedAskingPrice"].ToSafeDouble() : null;
            AdDate = row.Table.Columns.Contains("AdDate") ? row["AdDate"].ToSafeMinNullDate() : null;
            BidOffDate = row.Table.Columns.Contains("BidOffDate") ? row["BidOffDate"].ToSafeMinNullDate() : null;
            LastDateToApply = row.Table.Columns.Contains("LastDateToApply") ? row["LastDateToApply"].ToSafeMinNullDate() : null;
            Acreage = row.Table.Columns.Contains("Acreage") ? row["Acreage"].ToSafeDouble() : null;
            SquareFoot = row.Table.Columns.Contains("SquareFoot") ? row["SquareFoot"].ToSafeInt() : null;
            Dimensions = row.Table.Columns.Contains("Dimensions") ? row["Dimensions"].ToSafeString() : null;
            HasDemo = row.Table.Columns.Contains("HasDemo") ? row["HasDemo"].ToSafeBool() : null;
            PermitStatus = row.Table.Columns.Contains("PermitStatus") ? row["PermitStatus"].ToSafeString() : null;
            PropertyStatus = row.Table.Columns.Contains("PropertyStatus") ? row["PropertyStatus"].ToSafeString() : null;
            PropertyClassification = row.Table.Columns.Contains("PropertyClassification") ? row["PropertyClassification"].ToSafeString() : null;
            Source = row.Table.Columns.Contains("Source") ? row["Source"].ToSafeString() : null;
            Owner = row.Table.Columns.Contains("Owner") ? row["Owner"].ToSafeString() : null;
            CreatedDate = row.Table.Columns.Contains("CreatedDate") ? row["CreatedDate"].ToSafeMinNullDate() : null;
            UpdatedDate = row.Table.Columns.Contains("UpdatedDate") ? row["UpdatedDate"].ToSafeMinNullDate() : null;
            CreatedBy = row.Table.Columns.Contains("CreatedBy") ? row["CreatedBy"].ToSafeString() : null;
            UpdatedBy = row.Table.Columns.Contains("UpdatedBy") ? row["UpdatedBy"].ToSafeString() : null;

        }
    }
}
