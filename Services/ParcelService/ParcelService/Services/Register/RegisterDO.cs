using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Register
{
    public class RegisterDO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterDO() { }

        public RegisterDO(DataRow row)
        {
            Id = row.Table.Columns.Contains("Id") ? row["Id"].ToSafeInt() : 0;
            FullName = row.Table.Columns.Contains("FullName") ? row["FullName"].ToSafeString() : string.Empty;
            UserName = row.Table.Columns.Contains("UserName") ? row["UserName"].ToSafeString() : string.Empty;
            Email = row.Table.Columns.Contains("Email") ? row["Email"].ToSafeString() : string.Empty;
            Password = row.Table.Columns.Contains("Password") ? row["Password"].ToSafeString() : string.Empty ;
        }
    }
}
