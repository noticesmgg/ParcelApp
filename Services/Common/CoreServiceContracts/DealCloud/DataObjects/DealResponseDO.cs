using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Script.Lisp;

namespace CoreServiceContracts.DealCloud.DataObjects
{
    [Route("/Deals", "GET")]
    public class DealRequestDO : IReturn<DealResponseDO>    
    {
        public String ProjectName { get; set; }

        public int EntryId { get; set; }
    }

    public class DealResponseDO
    {
        public int Id { get; set; }

        public int DealId { get; set; }
        public int EntryId { get; set; }
        public String? ProjectName { get; set; }
        public bool? SPACDeal { get; set; }
        public DateTime? ABLDate { get; set; }
        public DateTime? ActiveDate { get; set; }
        public double? ActiveDays { get; set; }
        public double? AgencyFee { get; set; }
        public string? AgencyFeeFrequency { get; set; }
        public string? AmortizationFrequency { get; set; }
        public double? AmortizationYr1 { get; set; }
        public double? AmortizationYr2 { get; set; }
        public double? AmortizationYr3 { get; set; }
        public double? AmortizationYr4 { get; set; }
        public double? AmortizationYr5 { get; set; }
        public string? BalanceSheetTypes { get; set; }
        public string? BaseReference { get; set; }
        public string? BaseReference_Grid { get; set; }
        
        public string? ABLBaseReference { get; set; }
        public string? RevolverBaseReference { get; set; }
        public string? DDTLBaseReference { get; set; }
        public string? TermLoanBaseReference { get; set; }


        public double? Blended3YIRR { get; set; }
        public string? BoardRole { get; set; }
        public double? BoardSeats { get; set; }
        public string? CallProYr1 { get; set; }
        public string? CallProYr2 { get; set; }
        public string? CallProYr3 { get; set; }
        public string? CallProYr4 { get; set; }
        public string? CallProYr5 { get; set; }
        public DateTime? CapitalCallApprovedDate { get; set; }
        public string? CapitalCallApproved { get; set; }
        public DateTime? CapitalCallEstimatedDate { get; set; }
        public DateTime? CapitalCallEstimatedDateExpedited { get; set; }
        public DateTime? ClaimDate { get; set; }
        public DateTime? CommonEquityDate { get; set; }
        public String? CompanyName { get; set; }
        public double? CSA { get; set; }
        public DateTime? CurrentStageDate { get; set; }
        public DateTime? DDTLDate { get; set; }
        public double? DDTLTenor { get; set; }
        public String? DealDetails { get; set; }
        public string? DealLead { get; set; }
        public String? DealSource { get; set; }
        public String? DealSourceType { get; set; }
        public string? DealSponsor { get; set; }
        public String? DealTeam { get; set; }
        public DateTime? DebentureDate { get; set; }
        public string? DebtTenor { get; set; }
        public string? DifferentQualitiesPerInstrument { get; set; }
        public string? ECFDetailsI { get; set; }
        public string? ECFDetailsII { get; set; }
        public string? ECFDetailsIII { get; set; }
        public string? ECFFrequency { get; set; }
        public double? ECFI { get; set; }
        public double? ECFII { get; set; }
        public double? ECFIII { get; set; }
        public int? EquityCures { get; set; }
        public string? EquityCuresTerm { get; set; }
        public double? EquityOwnership { get; set; }
        public DateTime? ExclusivityExpirationDate { get; set; }
        public double? ExitFee { get; set; }
        public string? ExpenseDepositReceived { get; set; }
        public string? Features { get; set; }
        public string? FinancialCovenant { get; set; }

        public string? OtherCovenantDetails { get; set; }
        public double? Floor { get; set; }
        public string? FurthestActiveStage { get; set; }
        public int? DealPhaseId { get; set; }
        public string? GICSIndustry { get; set; }
        public string? GICSIndustryDefinition { get; set; }
        public string? GICSSector { get; set; }
        public string? InstrumentTypes { get; set; }
        public string? LegalCountryofOrigin { get; set; }
        public double? LeveragethroughMGG { get; set; }
        public double? LTMEBITDA { get; set; }
        public string? LTMEBITDACurrency { get; set; }
        public double? LTMRevenue { get; set; }
        public string? LTMRevenueCurrency { get; set; }
        public DateTime? MaxActiveDates { get; set; }
        public double? MGGCommittedABL { get; set; }
        public double? MGGCommittedClaim { get; set; }
        public double? MGGCommittedCommon { get; set; }
        public double? MGGCommittedDDTL { get; set; }
        public double? MGGCommittedDebenture { get; set; }
        public double? MGGCommittedNote { get; set; }
        public double? MGGCommittedOptions { get; set; }
        public double? MGGCommittedPreferred { get; set; }
        public double? MGGCommittedReceivable { get; set; }
        public double? MGGCommittedRevolver { get; set; }
        public double? MGGCommittedSecuritization { get; set; }
        public double? MGGCommittedTermLoan { get; set; }
        public double? ExercisePrice { get; set; }
        public string? MGGRole { get; set; }
        public String? MGGSource { get; set; }
        public double? MOIC { get; set; }
        public double? Multiple { get; set; }
        public double? OID { get; set; }
        public DateTime? NoteDate { get; set; }
        public DateTime? OptionsDate { get; set; }
        public double? PreferredEquityConversionRate { get; set; }
        public DateTime? PreferredEquityDate { get; set; }
        public double? PreferredEquityDividendRate { get; set; }
        public double? PreferredEquityLiquidationPreference { get; set; }
        public double? PreferredEquityOwnership { get; set; }
        public string? ProceedsDetails { get; set; }
        public DateTime? RecapitalizationDate { get; set; }
        public DateTime? ReceivableDate { get; set; }
        public DateTime? RevolverDate { get; set; }
        public double? RevolverTenor { get; set; }
        public double? ABLTenor { get; set; }

        public double? TermLoanTenor { get; set; }

        public DateTime? SecuritizationDate { get; set; }
        public string? SpreadDetailsI { get; set; }
        public string? SpreadDetailsII { get; set; }
        public string? SpreadDetailsIII { get; set; }
        public string? SpreadDetailsIV { get; set; }

        public string? EffectiveDateSpreadDetails { get; set; }
        public string? SpreadFrequency { get; set; }
        public string? SpreadFrequency_Grid { get; set; }
        public double? SpreadI { get; set; }
        public double? SpreadII { get; set; }
        public double? SpreadIII { get; set; }
        public double? SpreadIV { get; set; }

        public double? EffectiveDateSpread { get; set; }
        public string? Stage { get; set; }
        public string? StageDaysReference { get; set; }
        public DateTime? StageModifiedDate { get; set; }
        public string? Status { get; set; }
        public DateTime? StatusModifiedDate { get; set; }
        public DateTime? TermLoanDate { get; set; }
        public string? TermLoanLien { get; set; }
        public DateTime? TermSheetSignedDate { get; set; }
        public bool? TermSheetSigned { get; set; }
        public double? TotalCommitted { get; set; }
        public string? TotalCommittedCurrency { get; set; }
        public double? TotalCommittedABL { get; set; }
        public double? TotalCommittedClaim { get; set; }
        public double? TotalCommittedCommon { get; set; }
        public double? TotalCommittedDDTL { get; set; }
        public double? TotalCommittedDebenture { get; set; }
        public double? TotalCommittedNote { get; set; }
        public double? TotalCommittedOptions { get; set; }
        public double? TotalCommittedPreferred { get; set; }
        public double? TotalCommittedReceivable { get; set; }
        public double? TotalCommittedRevolver { get; set; }
        public double? TotalCommittedSecuritization { get; set; }
        public double? TotalCommittedTermLoan { get; set; }
        public double? MGGCommittedWarrants { get; set; }
        public double? TotalLeverage { get; set; }
        public double? TotalMGGCommitted { get; set; }
        public string? TotalMGGCommittedCurrency { get; set; }
        public double? TotalMGGOutstanding { get; set; }
        public string? TotalMGGOutstandingCurrency { get; set; }
        public double? TotalOutstanding { get; set; }
        public string? TotalOutstandingCurrency { get; set; }
        public DateTime? TransactionTypeDate { get; set; }
        public string? TransactionTypes { get; set; }
        public double? UnusedDDTLFee { get; set; }
        public string? UnusedDDTLFeeFrequency { get; set; }
        public double? UnusedRevolverFee { get; set; }
        public string? UnusedRevolverFeeFrequency { get; set; }
        public DateTime? WarrantsDate { get; set; }
        public string? DDTLCallProSchedule { get; set; }
      
        public String? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public String? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public double? RevolverCSA { get; set; }
        public double? DDTLCSA { get; set; }
        public double? ABLCSA { get; set; }
        public double? TermLoanCSA { get; set; }
        public double? PIK { get; set; }
        public double? TermLoanPIK { get; set; }
        public DateTime? CapitalCallApprovalDateExpedited { get; set; }
        public DateTime? NADate { get; set; }
        public DateTime? NewDate { get; set; }
        public int? NewDays { get; set; }
        public DateTime? PriorityDate { get; set; }
        public int? PriorityDays { get; set; }
        public DateTime? GreenlightDate { get; set; }
        public int? GreenlightDays { get; set; }
        public DateTime? PassedInitialICDiligenceDate { get; set; }
        public int? PassedInitialICDays { get; set; }
        public DateTime? ApprovedandClosingDate { get; set; }
        public int? ApprovedandClosingDays { get; set; }
        public String? CommonName { get; set; }
        public String? DealCaptain { get; set; }
        public String? Industry { get; set; }
        public String? Security { get; set; }
        public String? TransactionType { get; set; }
        public DateTime? CurrentInvestmentDate { get; set; }
        public DateTime? MGGInvestedDate { get; set; }
        public DateTime? PassedDate { get; set; }
        public double? TrancheSize { get; set; }

        public double? ECFConstant { get; set; }
        public double? AmortizationConstant { get; set; }
        public double? ManagementFee { get; set; }
        public string? ManagementFeeFrequency { get; set; }
        public double? SpreadConstant { get; set; }
        public String? CommonEquityDividendRate { get; set; }
        public double? CommonEquityOwnership { get; set; }

        public string? CommonEquityYieldRate { get; set; }    
        public double? ABLOID { get; set; }
        public double? RevolverOID { get; set; }
        public double? DDTLOID { get; set; }
        public double? TermLoanOID { get; set; }
        public double? ABLFloor { get; set; }
        public double? DDTLFloor { get; set; }
        public double? RevolverFloor { get; set; }
        public double? TermLoanFloor { get; set; }
        public string? ABLSpreadFrequency { get; set; }
        public string? DDTLSpreadFrequency { get; set; }
        public string? RevolverSpreadFrequency { get; set; }
        public string? TermLoanSpreadFrequency { get; set; }
        public double? ABLSpread { get; set; }
        public double? DDTLSpread { get; set; }
        public double? RevolverSpread { get; set; }
        public double? TermLoanSpread { get; set; }

        public string? YearBusinessStarted { get; set; }

        public bool? IsCurrent { get; set; } = true;

        public string? Website { get; set; }
        public string? Headquarters { get; set; }
        public bool? DDTLIsDiscretionary { get; set; }

        public double? ARR { get; set; }
        public string? ARRCurrency { get; set; }
        public double? ARRLeverageThroughMGG { get; set; }
        public double? TotalARRLeverage { get; set; }

        public string? PostCloseDeals { get; set; }

        public DealResponseDO()
        {

        }
    }
}
