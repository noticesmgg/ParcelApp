using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Parcels
{
    [Route("/parcels","GET")]
    public class ParcelsRequest : IReturn<ParcelsDO[]>
    {
    }
}
