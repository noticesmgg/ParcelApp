using Microsoft.AspNetCore.Mvc;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LandBank
{
    public class LandBankService : Service
    {
        public ILandBankDataProvider DataProvider { get; set; }
        public LandBankDO[] Get(LandBankRequest request)
        {
            Console.WriteLine($"Get landbank request started at {DateTime.Now:HH:mm:ss.fff}");
            var secmaster = DataProvider.Get();
            Console.WriteLine($"Get landbank request completed at {DateTime.Now:HH:mm:ss.fff}");
            return secmaster;
        }

        public bool Put(UpdateLandBank updateLandBank)
        {
            Console.WriteLine($"Put landbank request started at {DateTime.Now:HH:mm:ss.fff}");
            var result = DataProvider.Put(updateLandBank);
            Console.WriteLine($"Put landbank request completed at {DateTime.Now:HH:mm:ss.fff}");
            return result;
        }
    }
}
