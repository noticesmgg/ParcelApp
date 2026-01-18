using SharedCore.DB;
using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Register
{
    public class RegisterDataProvider : IRegisterDataProvider
    {
        public RegisterResponse Post(RegisterRequest registerRequest)
        {
            RegisterResponse registerResponse = new RegisterResponse();
            try
            {
                string strCheck = @$"Select * from LB_Users where Email = {registerRequest.Email.ToSQ()}";
                DataTable dt = Database.Instance.DB.GetRecords(strCheck);
                if (dt != null && dt.Rows.Count > 0)
                {
                    registerResponse.Message = "Given email id is already present";
                }
                else
                {
                    string strInsertSql = @$"Insert into LB_Users(FullName,UserName,Email,Password,CreatedDate,CreatedBy) values ({registerRequest.FullName.ToSQ()} ,{registerRequest.UserName.ToSQ()},{registerRequest.Email.ToSQ()},
										{registerRequest.Password.ToSQ()},{DateTime.Now.ToSQ()},{Environment.UserName.ToSQ()})";

                    int Save = Database.Instance.DB.Execute(strInsertSql);
                    registerResponse.Success = Save == 1;
                    registerResponse.Message = "User Register Successfully";
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error while saving user data :" + ex);
                registerResponse.Message = "User Register Failed";
            }
            return registerResponse;
        }
    }
}
