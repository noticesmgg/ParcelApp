using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.Hub.Sale
{
    [Route("/sale", "GET POST")]
    public class SaleRequest : IReturn<SaleDO[]>
    {
    }
}
