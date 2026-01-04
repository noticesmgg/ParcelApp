using ServiceStack;

namespace CoreServiceContracts.Hub.SecMaster
{
    [Route("/secmaster", "GET POST")]
    public class SecMasterRequest : IReturn<SecMasterDO[]>
    {
    }
}
