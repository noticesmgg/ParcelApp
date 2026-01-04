using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ParcelService.Services.Parcels
{
    public class ParcelsDO
    {
        public int LandBankId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public bool HasDemo { get; set; }
        public string Dimensions { get; set; }
        public string Notes { get; set; }
        public string PermitStatus { get; set; }
        public DateTime LastDateToApply { get; set; }
        public double Acreage { get; set; }
        public int SquareFoot { get; set; }
        public string PropertyStatus { get; set; }
        public string PropertyClassification { get; set; }
        public string Owner { get; set; }
        public string Source { get; set; }
        public string ParcelNumber { get; set; }
        public string ShortParcel { get; set; }
        public double AskingPrice { get; set; }
        public double UpdatedAskingPrice { get; set; }
        public string IsInterested { get; set; }
        public string ApplicationStatus { get; set; }
        public string ApplicationNumber { get; set; }
        public string SubmittedAccount { get; set; }
        public double OurBid { get; set; }
        public string Competitor { get; set; }
        public double WinningBid { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime AcceptedDate { get; set; }
        public double UpperLimit { get; set; }
        public string CompLimit { get; set; }
        public DateTime AdDate { get; set; }
        public DateTime BidOffDate { get; set; }
        public string AccessorLink { get; set; }
        public string GisLink { get; set; }
        public string RegistryLink { get; set; }

        public ParcelsDO()
        {
        }

        public ParcelsDO(DataRow row)
        {
            LandBankId = row.Table.Columns.Contains("LandBankId") ? row["LandBankId"].ToSafeInt() : 0;
            Street = row.Table.Columns.Contains("Street") ? row["Street"].ToSafeString() : string.Empty;
            City = row.Table.Columns.Contains("City") ? row["City"].ToSafeString() : string.Empty;
            State = row.Table.Columns.Contains("State") ? row["State"].ToSafeString() : string.Empty;
            ZipCode = row.Table.Columns.Contains("ZipCode") ? row["ZipCode"].ToSafeInt() : 0;
            HasDemo = row.Table.Columns.Contains("HasDemo") ? row["HasDemo"].ToSafeBool() : false;
            Dimensions = row.Table.Columns.Contains("Dimensions") ? row["Dimensions"].ToSafeString() : string.Empty;
            Notes = row.Table.Columns.Contains("Notes") ? row["Notes"].ToSafeString() : string.Empty;
            PermitStatus = row.Table.Columns.Contains("PermitStatus") ? row["PermitStatus"].ToSafeString() : string.Empty;
            LastDateToApply = row.Table.Columns.Contains("LastDateToApply") ? row["LastDateToApply"].ToSafeMinNullDate() ?? DateTime.MinValue : DateTime.MinValue;
            Acreage = row.Table.Columns.Contains("Acreage") ? row["Acreage"].ToSafeDouble() : 0.0;
            SquareFoot = row.Table.Columns.Contains("SquareFoot") ? row["SquareFoot"].ToSafeInt() : 0;
            PropertyStatus = row.Table.Columns.Contains("PropertyStatus") ? row["PropertyStatus"].ToSafeString() : string.Empty;
            PropertyClassification = row.Table.Columns.Contains("PropertyClassification") ? row["PropertyClassification"].ToSafeString() : string.Empty;
            Owner = row.Table.Columns.Contains("Owner") ? row["Owner"].ToSafeString() : string.Empty;
            Source = row.Table.Columns.Contains("Source") ? row["Source"].ToSafeString() : string.Empty;
            ParcelNumber = row.Table.Columns.Contains("ParcelNumber") ? row["ParcelNumber"].ToSafeString() : string.Empty;
            ShortParcel = row.Table.Columns.Contains("ShortParcel") ? row["ShortParcel"].ToSafeString() : string.Empty;
            AskingPrice = row.Table.Columns.Contains("AskingPrice") ? row["AskingPrice"].ToSafeDouble() : 0.0;
            UpdatedAskingPrice = row.Table.Columns.Contains("UpdatedAskingPrice") ? row["UpdatedAskingPrice"].ToSafeDouble() : 0.0;
            IsInterested = row.Table.Columns.Contains("IsInterested") ? row["IsInterested"].ToSafeString() : string.Empty;
            ApplicationStatus = row.Table.Columns.Contains("ApplicationStatus") ? row["ApplicationStatus"].ToSafeString() : string.Empty;
            ApplicationNumber = row.Table.Columns.Contains("ApplicationNumber") ? row["ApplicationNumber"].ToSafeString() : string.Empty;
            SubmittedAccount = row.Table.Columns.Contains("SubmittedAccount") ? row["SubmittedAccount"].ToSafeString() : string.Empty;
            OurBid = row.Table.Columns.Contains("OurBid") ? row["OurBid"].ToSafeDouble() : 0.0;
            Competitor = row.Table.Columns.Contains("Competitor") ? row["Competitor"].ToSafeString() : string.Empty;
            WinningBid = row.Table.Columns.Contains("WinningBid") ? row["WinningBid"].ToSafeDouble() : 0.0;
            SubmitDate = row.Table.Columns.Contains("SubmitDate") ? row["SubmitDate"].ToSafeMinNullDate() ?? DateTime.MinValue : DateTime.MinValue;
            AcceptedDate = row.Table.Columns.Contains("AcceptedDate") ? row["AcceptedDate"].ToSafeMinNullDate() ?? DateTime.MinValue : DateTime.MinValue;
            UpperLimit = row.Table.Columns.Contains("UpperLimit") ? row["UpperLimit"].ToSafeDouble() : 0.0;
            CompLimit = row.Table.Columns.Contains("CompLimit") ? row["CompLimit"].ToSafeString() : string.Empty;
            AdDate = row.Table.Columns.Contains("AdDate") ? row["AdDate"].ToSafeMinNullDate() ?? DateTime.MinValue : DateTime.MinValue;
            BidOffDate = row.Table.Columns.Contains("BidOffDate") ? row["BidOffDate"].ToSafeMinNullDate() ?? DateTime.MinValue : DateTime.MinValue;
            AccessorLink = row.Table.Columns.Contains("AccessorLink") ? row["AccessorLink"].ToSafeString() : string.Empty;
            GisLink = row.Table.Columns.Contains("GisLink") ? row["GisLink"].ToSafeString() : string.Empty;
            RegistryLink = row.Table.Columns.Contains("RegistryLink") ? row["RegistryLink"].ToSafeString() : string.Empty;
        }
    }
}
