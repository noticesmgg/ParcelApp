using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBBookmarks
{
    [Route("/lbbookmarks", "GET POST")]
    public class BookmarksRequest : IReturn<BookmarksDO>
    {
        public int LandBankId { get; set; }
    }
}
