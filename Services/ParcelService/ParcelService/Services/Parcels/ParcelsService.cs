using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Parcels
{
    public class ParcelsService : Service
    {
        public IParcelsDataProvider parcelsDataProvider { get; set; }

        public ParcelsDO[] Get(ParcelsRequest request)
        {
            Console.WriteLine($"Get parcels request started at {DateTime.Now:HH:mm:ss.fff}");
            var parcels = parcelsDataProvider.Get();
            Console.WriteLine($"Get parcels request completed at {DateTime.Now:HH:mm:ss.fff}");
            return parcels;
        }
    }
}
