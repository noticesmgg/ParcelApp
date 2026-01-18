using SharedCore.DB;
using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Login
{
    public class LoginDataProvider : ILoginDataProvider
    {
        public LoginResponse Post(LoginRequest loginRequest)
        {
			LoginResponse response = new();
			try
			{
				string strlogin = @$"select * from LB_Users where UserName = {loginRequest.UserName.ToSQ()} and Password = {loginRequest.Password.ToSQ()}";
                DataTable dt = Database.Instance.DB.GetRecords(strlogin);
                if (dt != null && dt.Rows.Count > 0)
                {
                    response.Success = true;
                    response.Message = "User login Successfully";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Invalid username or password";
                }
            }
			catch (Exception ex)
			{
                Logger.Error("Error while login" + ex);
                response.Message = "Error while login";
			}
			return response;
        }
    }
}
