using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LandBank
{
    [Route("/landbank", "GET")]
    public class LandBankRequest : IReturn<LandBankDO[]>
    {
    }

    [Route("/landbankbyid","GET")]
    public class LandBankRequestById : IReturn<LandBankDO>
    {
        public int LandBankId { get; set; }
    }

    [Route("/landbank/{Id}", "PUT")]
    public class UpdateLandBank : IReturn<bool>
    {
        public int Id { get; set; }
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
    }

    [Route("/landbank/uploads", "POST")]
    public class LandBankUploads : IReturn<bool>
    {
        public string  LandBankId { get; set; }

    }

}
