using CoreServiceContracts;
using CoreServiceContracts.HeartbeatApi.DataObjects;
using ServiceStack;
using System;
using System.Data;

namespace CoreServiceContracts.Hub.SecMaster
{
#pragma warning disable CS8618
    public class SecMasterDO
    {
        public string SecurityName { get; set; }
        public string AssetType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectIRRGrouping { get; set; }
        public string SecurityIRRGrouping { get; set; }
        public string Vintage { get; set; }
        public string POName { get; set; }
        public string IRRGroupingNameDefault { get; set; }
        public string CommonName { get; set; }
        public string IRRGroupingNameOverride { get; set; }
        public string Status_Sec { get; set; }
        public string Status_Proj { get; set; }
        public int? StatusID { get; set; }
        public int? ProjectID { get; set; }
        public int? PfoCompanyId { get; set; }
        public bool? isNew { get; set; }
        public string Borrower { get; set; }
        public int? SecurityID { get; set; }
        public string SSC_ID { get; set; }
        public string AliasUSBank { get; set; }
        public string AliasAlterDomus { get; set; }
        public string AliasStateStreet { get; set; }
        public string AliasMarkit { get; set; }
        public string AliasCentaur { get; set; }
        public string VPMCode { get; set; }
        public string VPMDescription { get; set; }
        public string VPMRollUpName { get; set; }
        public bool? IsSPV { get; set; }
        public string LinkedSymbol { get; set; }
        public string VPMSecType { get; set; }
        public int? SymbolID { get; set; }
        public int? RepSecurityTypeID { get; set; }
        public bool? IgnoreFoxLookthrough { get; set; }
        public string IsFunded { get; set; }
        public string AdminCode { get; set; }
        public string ExposureCCY { get; set; }
        public DateTime? MaturityDateSecurity { get; set; }
        public string SecurityType { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string AmortizationRepaymentType { get; set; }
        public string AmortizationFrequencyType { get; set; }
        public string RepSecurityTypeAtInvest { get; set; }
        public int? FeederSecurityID { get; set; }
        public string RepSecurityType { get; set; }
        public bool? IsDiscretionary { get; set; }
        public string SecurityTypeCode { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? IssueDate { get; set; }
        public double Multiplier { get; set; }
        public double RoundLot { get; set; }
        public double BookBondPremiumMarketDiscount { get; set; }
        public double BookOIDAcquisitionPremium { get; set; }
        public string AmortizationMethod { get; set; }
        public double IssuePrice { get; set; }
        public DateTime? MaturityDateLegal { get; set; }
        public bool? IsPending { get; set; }
        public int? SecurityTypeID { get; set; }
        public int? POID { get; set; }
        public string Obligor { get; set; }
        public string BalanceSheetType { get; set; }
        public string InstrumentType { get; set; }
        public string SecuredStatus { get; set; }
        public string EquityType { get; set; }
        public string CapitalStack { get; set; }
        public string LienOrderDetail { get; set; }
        public string InstrumentPriming { get; set; }
        public string NoteType { get; set; }
        public string AssetTypeReporting { get; set; }
        public string AssetTypePO { get; set; }
        public string AssetTypeIRR { get; set; }
        public string AssetTypeVal { get; set; }
        public string SecurityIRRGroupingProposal { get; set; }
        public string SOI { get; set; }
        public string PONameProposal { get; set; }
        public string AssociateCode { get; set; }
        public bool? IsMaturityDateOverride { get; set; }
        public string Rating { get; set; }
        public int RowIndex { get; set; }

        public SecMasterDO() { }

        public SecMasterDO(DataRow row)
        {
            SecurityName = row.Table.Columns.Contains("SecurityName") ? row["SecurityName"].ToSafeString() : null;
            AssetType = row.Table.Columns.Contains("AssetType") ? row["AssetType"].ToSafeString() : null;
            ProjectName = row.Table.Columns.Contains("ProjectName") ? row["ProjectName"].ToSafeString() : null;
            ProjectIRRGrouping = row.Table.Columns.Contains("ProjectIRRGrouping") ? row["ProjectIRRGrouping"].ToSafeString() : null;
            SecurityIRRGrouping = row.Table.Columns.Contains("SecurityIRRGrouping") ? row["SecurityIRRGrouping"].ToSafeString() : null;
            Vintage = row.Table.Columns.Contains("Vintage") ? row["Vintage"].ToSafeString() : null;
            POName = row.Table.Columns.Contains("POName") ? row["POName"].ToSafeString() : null;
            IRRGroupingNameDefault = row.Table.Columns.Contains("IRRGroupingNameDefault") ? row["IRRGroupingNameDefault"].ToSafeString() : null;
            CommonName = row.Table.Columns.Contains("CommonName") ? row["CommonName"].ToSafeString() : null;
            IRRGroupingNameOverride = row.Table.Columns.Contains("IRRGroupingNameOverride") ? row["IRRGroupingNameOverride"].ToSafeString() : null;
            Status_Sec = row.Table.Columns.Contains("Status (Sec)") ? row["Status (Sec)"].ToSafeString() : null;
            Status_Proj = row.Table.Columns.Contains("Status (Proj)") ? row["Status (Proj)"].ToSafeString() : null;
            StatusID = row.Table.Columns.Contains("StatusID") ? row["StatusID"].ToSafeString().ToSafeInt() : (int?)null;
            ProjectID = row.Table.Columns.Contains("ProjectID") ? row["ProjectID"].ToSafeString().ToSafeInt() : (int?)null;
            PfoCompanyId = row.Table.Columns.Contains("PfoCompanyId") ? row["PfoCompanyId"].ToSafeString().ToSafeInt() : (int?)null;
            isNew = row.Table.Columns.Contains("isNew") ? row["isNew"].ToSafeString().ToSafeBool() : false;
            Borrower = row.Table.Columns.Contains("Borrower") ? row["Borrower"].ToSafeString() : null;
            SecurityID = row.Table.Columns.Contains("SecurityID") ? row["SecurityID"].ToSafeString().ToSafeInt() : (int?)null;
            SSC_ID = row.Table.Columns.Contains("SSC_ID") ? row["SSC_ID"].ToSafeString() : null;
            AliasUSBank = row.Table.Columns.Contains("AliasUSBank") ? row["AliasUSBank"].ToSafeString() : null;
            AliasAlterDomus = row.Table.Columns.Contains("AliasAlterDomus") ? row["AliasAlterDomus"].ToSafeString() : null;
            AliasStateStreet = row.Table.Columns.Contains("AliasStateStreet") ? row["AliasStateStreet"].ToSafeString() : null;
            AliasMarkit = row.Table.Columns.Contains("AliasMarkit") ? row["AliasMarkit"].ToSafeString() : null;
            AliasCentaur = row.Table.Columns.Contains("AliasCentaur") ? row["AliasCentaur"].ToSafeString() : null;
            VPMCode = row.Table.Columns.Contains("VPMCode") ? row["VPMCode"].ToSafeString() : null;
            VPMDescription = row.Table.Columns.Contains("VPMDescription") ? row["VPMDescription"].ToSafeString() : null;
            VPMRollUpName = row.Table.Columns.Contains("VPMRollUpName") ? row["VPMRollUpName"].ToSafeString() : null;
            IsSPV = row.Table.Columns.Contains("IsSPV") ? row["IsSPV"].ToSafeString().ToSafeBool() : false;
            LinkedSymbol = row.Table.Columns.Contains("LinkedSymbol") ? row["LinkedSymbol"].ToSafeString() : null;
            VPMSecType = row.Table.Columns.Contains("VPMSecType") ? row["VPMSecType"].ToSafeString() : null;
            SymbolID = row.Table.Columns.Contains("SymbolID") ? row["SymbolID"].ToSafeString().ToSafeInt() : (int?)null;
            RepSecurityTypeID = row.Table.Columns.Contains("RepSecurityTypeID") ? row["RepSecurityTypeID"].ToSafeString().ToSafeInt() : (int?)null;
            IgnoreFoxLookthrough = row.Table.Columns.Contains("IgnoreFoxLookthrough") ? row["IgnoreFoxLookthrough"].ToSafeString().ToSafeBool() : false;
            IsFunded = row.Table.Columns.Contains("IsFunded") ? row["IsFunded"].ToSafeString() : null;
            AdminCode = row.Table.Columns.Contains("AdminCode") ? row["AdminCode"].ToSafeString() : null;
            ExposureCCY = row.Table.Columns.Contains("ExposureCCY") ? row["ExposureCCY"].ToSafeString() : null;
            MaturityDateSecurity = row.Table.Columns.Contains("MaturityDateSecurity") ? row["MaturityDateSecurity"].ToSafeMinNullDate() : null;
            SecurityType = row.Table.Columns.Contains("SecurityType") ? row["SecurityType"].ToSafeString() : null;
            MaturityDate = row.Table.Columns.Contains("MaturityDate") ? row["MaturityDate"].ToSafeMinNullDate() : null;
            AmortizationRepaymentType = row.Table.Columns.Contains("AmortizationRepaymentType") ? row["AmortizationRepaymentType"].ToSafeString() : null;
            AmortizationFrequencyType = row.Table.Columns.Contains("AmortizationFrequencyType") ? row["AmortizationFrequencyType"].ToSafeString() : null;
            RepSecurityTypeAtInvest = row.Table.Columns.Contains("RepSecurityTypeAtInvest") ? row["RepSecurityTypeAtInvest"].ToSafeString() : null;
            FeederSecurityID = row.Table.Columns.Contains("FeederSecurityID") ? row["FeederSecurityID"].ToSafeString().ToSafeInt() : (int?)null;
            RepSecurityType = row.Table.Columns.Contains("RepSecurityType") ? row["RepSecurityType"].ToSafeString() : null;
            IsDiscretionary = row.Table.Columns.Contains("IsDiscretionary") ? row["IsDiscretionary"].ToSafeString().ToSafeBool() : false;
            SecurityTypeCode = row.Table.Columns.Contains("SecurityTypeCode") ? row["SecurityTypeCode"].ToSafeString() : null;
            LastUpdatedAt = row.Table.Columns.Contains("LastUpdatedAt") ? row["LastUpdatedAt"].ToSafeMinNullDate() : null;
            LastUpdated = row.Table.Columns.Contains("LastUpdated") ? row["LastUpdated"].ToSafeMinNullDate() : null;
            LastUpdatedBy = row.Table.Columns.Contains("LastUpdatedBy") ? row["LastUpdatedBy"].ToSafeString() : null;
            IssueDate = row.Table.Columns.Contains("IssueDate") ? row["IssueDate"].ToSafeMinNullDate() : null;
            Multiplier = row.Table.Columns.Contains("Multiplier") ? row["Multiplier"].ToSafeDouble() : 0.0;
            RoundLot = row.Table.Columns.Contains("RoundLot") ? row["RoundLot"].ToSafeDouble() : 0.0;
            BookBondPremiumMarketDiscount = row.Table.Columns.Contains("BookBondPremiumMarketDiscount") ? row["BookBondPremiumMarketDiscount"].ToSafeDouble() : 0.0;
            BookOIDAcquisitionPremium = row.Table.Columns.Contains("BookOIDAcquisitionPremium") ? row["BookOIDAcquisitionPremium"].ToSafeDouble() : 0.0;
            AmortizationMethod = row.Table.Columns.Contains("AmortizationMethod") ? row["AmortizationMethod"].ToSafeString() : null;
            IssuePrice = row.Table.Columns.Contains("IssuePrice") ? row["IssuePrice"].ToSafeDouble() : 0.0;
            MaturityDateLegal = row.Table.Columns.Contains("MaturityDateLegal") ? row["MaturityDateLegal"].ToSafeMinNullDate() : null;
            IsPending = row.Table.Columns.Contains("IsPending") ? row["IsPending"].ToSafeString().ToSafeBool() : false;
            SecurityTypeID = row.Table.Columns.Contains("SecurityTypeID") ? row["SecurityTypeID"].ToSafeString().ToSafeInt() : (int?)null;
            POID = row.Table.Columns.Contains("POID") ? row["POID"].ToSafeString().ToSafeInt() : (int?)null;
            Obligor = row.Table.Columns.Contains("Obligor") ? row["Obligor"].ToSafeString() : null;
            BalanceSheetType = row.Table.Columns.Contains("BalanceSheetType") ? row["BalanceSheetType"].ToSafeString() : null;
            InstrumentType = row.Table.Columns.Contains("InstrumentType") ? row["InstrumentType"].ToSafeString() : null;
            SecuredStatus = row.Table.Columns.Contains("SecuredStatus") ? row["SecuredStatus"].ToSafeString() : null;
            EquityType = row.Table.Columns.Contains("EquityType") ? row["EquityType"].ToSafeString() : null;
            CapitalStack = row.Table.Columns.Contains("CapitalStack") ? row["CapitalStack"].ToSafeString() : null;
            LienOrderDetail = row.Table.Columns.Contains("LienOrderDetail") ? row["LienOrderDetail"].ToSafeString() : null;
            InstrumentPriming = row.Table.Columns.Contains("InstrumentPriming") ? row["InstrumentPriming"].ToSafeString() : null;
            NoteType = row.Table.Columns.Contains("NoteType") ? row["NoteType"].ToSafeString() : null;
            AssetTypeReporting = row.Table.Columns.Contains("AssetTypeReporting") ? row["AssetTypeReporting"].ToSafeString() : null;
            AssetTypePO = row.Table.Columns.Contains("AssetTypePO") ? row["AssetTypePO"].ToSafeString() : null;
            AssetTypeIRR = row.Table.Columns.Contains("AssetTypeIRR") ? row["AssetTypeIRR"].ToSafeString() : null;
            AssetTypeVal = row.Table.Columns.Contains("AssetTypeVal") ? row["AssetTypeVal"].ToSafeString() : null;
            SecurityIRRGroupingProposal = row.Table.Columns.Contains("SecurityIRRGroupingProposal") ? row["SecurityIRRGroupingProposal"].ToSafeString() : null;
            SOI = row.Table.Columns.Contains("SOI") ? row["SOI"].ToSafeString() : null;
            PONameProposal = row.Table.Columns.Contains("PONameProposal") ? row["PONameProposal"].ToSafeString() : null;
            AssociateCode = row.Table.Columns.Contains("AssociateCode") ? row["AssociateCode"].ToSafeString() : null;
            IsMaturityDateOverride = row.Table.Columns.Contains("IsMaturityDateOverride") ? row["IsMaturityDateOverride"].ToSafeString().ToSafeBool() : false;
            Rating = row.Table.Columns.Contains("Rating") ? row["Rating"].ToSafeString() : null;
            RowIndex = row.Table.Columns.Contains("RowIndex") ? row["RowIndex"].ToSafeInt() : 0;

        }


    }
#pragma warning restore CS8618
}