using ServiceStack.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LandBank
{
    public interface ILandBankDataProvider
    {
        LandBankDO[] Get();

        bool Put(UpdateLandBank landBankDOs);

        bool Post(LandBankUploads landBankImages , IHttpFile[]? files);
    }
}
