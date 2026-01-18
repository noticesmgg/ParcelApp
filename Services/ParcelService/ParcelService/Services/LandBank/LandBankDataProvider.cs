using Microsoft.Extensions.Configuration;
using ServiceStack;
using ServiceStack.Web;
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
        public string connectionString = string.Empty;
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

        public LandBankDO Get(LandBankRequestById requestById)
        {
            string sql = $"Select * from LandBank where Id {requestById.LandBankId}";

            DataTable dt = Database.Instance.DB.GetRecords(sql);
            return dt.AsEnumerable()
                   .Select(dr => new LandBankDO(dr)).First();
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

        public  async Task<LandBankUploadResponse> Post(LandBankUploads landBankUpload,IHttpFile[]? files)
        {
            var result = new LandBankUploadResponse();

            if (files == null || files.Length == 0)
                throw HttpError.BadRequest("No files uploaded");

            try
            {
                var blobService = new BlobStorageService(connectionString,"landbank-files");

                foreach (var file in files)
                {
                    try
                    {
                        string fileUrl = await blobService.UploadAsync(file.InputStream,file.FileName,file.ContentType,landBankUpload.LandBankId);

                        bool isSave = SaveFileUrlToDatabase(fileUrl,file.ContentType,landBankUpload.LandBankId.ToSafeInt(),file.FileName);

                        if(isSave)
                            result.UploadedFilesUrl.Add(fileUrl);
                        else
                            result.FailedFiles.Add(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"File upload failed: {file.FileName}", ex);
                        result.FailedFiles.Add(file.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Upload process failed", ex);
            }
            return result;
        }


        public static bool SaveFileUrlToDatabase(string fileUrl, string mediaType, int landBankId, string fileName)
        {
            bool SaveSuccessfully = false;
            try
            {
                string sqlGet = $@"Select * from LB_Media where LandBankId = {landBankId} and FileName = {fileName.ToSQ()} and MediaType ={mediaType.ToSQ()}";
                DataTable dt = Database.Instance.DB.GetRecords(sqlGet);
                string sql;
                if (dt != null && dt.Rows.Count > 0)
                    sql = @$"Update LB_Media set FileUrl = {fileUrl.ToSQ()} ,UpdatedDate = {DateTime.Now.ToSQ()}, UpdatedBy = {Environment.UserName.ToSQ()} where 
                            LandBankId = {landBankId} and FileName = {fileName.ToSQ()} and MediaType = {mediaType.ToSQ()}";
                else
                    sql = @$"Insert into LB_Media(LandBankId,FileName,FileUrl,MediaType,CreatedDate,CreatedBy) values({landBankId},{fileName.ToSQ()},{fileUrl.ToSQ()},{mediaType.ToSQ()},{DateTime.Now.ToSQ()},{Environment.UserName.ToSQ()})";

                var result = Database.Instance.DB.Execute(sql);
                SaveSuccessfully = result == 1;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while saving image into database" + ex);
            }
            return SaveSuccessfully;
        }

    }
}
