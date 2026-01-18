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

        LandBankDO Get(LandBankRequestById requestById);

        bool Put(UpdateLandBank landBankDOs);

        Task<LandBankUploadResponse> Post(LandBankUploads landBankImages , IHttpFile[]? files);
    }
}
