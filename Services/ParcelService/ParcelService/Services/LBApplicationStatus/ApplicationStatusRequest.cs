using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBApplicationStatus
{
    [Route("/lbapplicationstatus", "GET")]
    public class ApplicationStatusRequest : IReturn<ApplicationStatusDO>
    {
        public int LandBankId { get; set; }
    }
}
