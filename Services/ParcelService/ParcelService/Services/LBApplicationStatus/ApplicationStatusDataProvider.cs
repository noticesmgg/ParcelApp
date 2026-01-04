using SharedCore.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBApplicationStatus
{
    public class ApplicationStatusDataProvider : IApplicationStatusDataProvider
    {
        ApplicationStatusDO IApplicationStatusDataProvider.Get(ApplicationStatusRequest request)
        {
            string sql = @$"select a.Id,a.LandBankId,a.AccountId,a.SubmitDate,a.ReSubmitDate,a.AcceptedDate,a.ApplicationNumber,s.StatusCode,a.OurBid,a.Competitor,
                            a.WinningBid,a.CreatedDate,a.CreatedBy,a.UpdatedDate,a.UpdatedBy from LB_ApplicationStatus a join LB_StatusCodes s on a.StatusCode = s.Id 
                            where a.LandBankId = {request.LandBankId}";

            DataTable dt = Database.Instance.DB.GetRecords(sql);

            return dt.Rows.Count > 0 ? dt.AsEnumerable()
                   .Select(dr => new ApplicationStatusDO(dr)).FirstOrDefault() : new ApplicationStatusDO { Id = request.LandBankId};
        }
    }
}
