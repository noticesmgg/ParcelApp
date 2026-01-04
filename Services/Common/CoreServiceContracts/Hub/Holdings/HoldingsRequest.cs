using ServiceStack;

namespace CoreServiceContracts.Hub.Holdings
{
    public enum RequestType { NA = 0, Deal = 1, IR = 2, Ops = 3, Mgmt = 4 }
    [Route("/holdings", "GET POST")]
    public class HoldingsRequest : IReturn<HoldingsDO[]>
    {
        public DateTime? AsOfDate { get; set; }        
        public RequestType RequestType { get; set; } = RequestType.NA;

        public bool Allocate { get; set; } = false;
        public bool RawHoldings { get; set; } = false;
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
