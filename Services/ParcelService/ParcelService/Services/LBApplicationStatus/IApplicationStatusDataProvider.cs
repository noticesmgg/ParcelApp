using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBApplicationStatus
{
    public interface IApplicationStatusDataProvider
    {
        ApplicationStatusDO Get(ApplicationStatusRequest request);
    }
}
