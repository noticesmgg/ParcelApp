using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LBBookmarks
{
    public class BookmarksService : Service
    {
       public IBookmarksDataProvider bookmarksDataProvider { get; set; }
        public BookmarksDO Get(BookmarksRequest request)
        {
            Console.WriteLine($"Get bookmarks request started at {DateTime.Now:HH:mm:ss.fff}");
            var bookmarks = bookmarksDataProvider.Get(request);
            Console.WriteLine($"Get bookmarks request completed at {DateTime.Now:HH:mm:ss.fff}");
            return bookmarks;
        }
    }
}
