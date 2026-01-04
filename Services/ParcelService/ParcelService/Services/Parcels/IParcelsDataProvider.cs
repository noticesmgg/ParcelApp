using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Parcels
{
    public interface IParcelsDataProvider
    {
        ParcelsDO[] Get();
    }
}
