using SharedCore.DB;
using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LandBank
{
    public class LandBankDataProvider : ILandBankDataProvider
    {
        public LandBankDataProvider()
        {
            Logger.Info("LandBankDataProvider invoked");
        }

        public LandBankDO[] Get()
        {
            string sql = "Select * from LandBank";

            DataTable dt = Database.Instance.DB.GetRecords(sql);
            return dt.AsEnumerable()
                   .Select(dr => new LandBankDO(dr)).ToArray();
        }                      

        public bool Put(UpdateLandBank landBankDOs)
        {
            try
            {
                string sql = $@"update LandBank set ShortParcel = {landBankDOs.ShortParcel.ToSQ()}, Street = {landBankDOs.Street.ToSQ()} , City = {landBankDOs.City.ToSQ()} , 
                                State ={landBankDOs.State.ToSQ()} , ZipCode = {landBankDOs.ZipCode} , AskingPrice = {landBankDOs.AskingPrice} , UpdatedAskingPrice = {landBankDOs.UpdatedAskingPrice}
                                , AdDate = {landBankDOs.AdDate.ToSQ()} , BidOffDate = {landBankDOs.BidOffDate.ToSQ()} , LastDateToApply = {landBankDOs.LastDateToApply.ToSQ()} ,
                                Acreage = {landBankDOs.Acreage} , SquareFoot = {landBankDOs.SquareFoot} , Dimensions = {landBankDOs.Dimensions.ToSQ()} , HasDemo = {landBankDOs.HasDemo.ToSafeBit()} ,
                                PermitStatus = {landBankDOs.PermitStatus.ToSQ()} , PropertyStatus = {landBankDOs.PropertyStatus.ToSQ()} , PropertyClassification = {landBankDOs.PropertyClassification.ToSQ()} ,
                                Source = {landBankDOs.Source.ToSQ()} , Owner = {landBankDOs.Owner.ToSQ()} , UpdatedDate = {DateTime.Now.ToSQ()} , UpdatedBy = {Environment.UserName.ToSQ()} where Id = {landBankDOs.Id}";

                var result = Database.Instance.DB.Execute(sql);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while save put request :", ex);
                return false;
            }
        }
    }
}
