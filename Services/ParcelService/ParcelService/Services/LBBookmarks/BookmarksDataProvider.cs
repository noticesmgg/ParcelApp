using SharedCore.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBBookmarks
{
    public class BookmarksDataProvider : IBookmarksDataProvider
    {
        BookmarksDO IBookmarksDataProvider.Get(BookmarksRequest request)
        {
            string sql = $@"select b.id,b.LandBankId,i.Status as Interest,b.UpperLimit,b.CompLimit,b.Notes,b.CreatedDate,b.CreatedBy,b.UpdatedDate,b.UpdatedBy from LB_Bookmarks b 
                            join LB_InterestCodes i on b.InterestId = i.Id where b.LandBankId = {request.LandBankId}";

            DataTable dt = Database.Instance.DB.GetRecords(sql);

            return dt.Rows.Count > 0 ? dt.AsEnumerable()
                   .Select(dr => new BookmarksDO(dr)).FirstOrDefault() : new BookmarksDO { Id = request.LandBankId };
        }
    }
}
