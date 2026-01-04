using SharedCore.DB;
using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Parcels
{
    public class ParcelsDataProvider : IParcelsDataProvider
    {
        public ParcelsDataProvider() 
        { 
            Logger.Info("ParcelsDataProvider initialized.");
        }
        public ParcelsDO[] Get()
        {
			try
			{
                string sql = "Select * from vw_Parcels";
                DataTable dt = Database.Instance.DB.GetRecords(sql);
                return dt.AsEnumerable()
                       .Select(dr => new ParcelsDO(dr)).ToArray();
            }
			catch (Exception  ex)
			{
                Logger.Error("Error while getting parcels data :", ex);
                return Array.Empty<ParcelsDO>();
            }
        }
    }
}
