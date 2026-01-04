using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBApplicationStatus
{
    public class ApplicationStatusService : Service
    {
        public IApplicationStatusDataProvider applicationStatusDataProvider { get; set; }

        public ApplicationStatusDO Get(ApplicationStatusRequest request)
        {
            Console.WriteLine($"Get application status request started at {DateTime.Now:HH:mm:ss.fff}");
            ApplicationStatusDO applicationStatus = applicationStatusDataProvider.Get(request);
            Console.WriteLine($"Get application status request completed at {DateTime.Now:HH:mm:ss.fff}");
            return applicationStatus;
        }
    }
}
