using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.Hub.Menu
{
    /// <summary>
    /// Represents a user permission record (from usp_get_Perms).
    /// </summary>
    public class PermissionDO
    {
        public string PermissionKey { get; set; } = string.Empty;
        public bool IsReadOnly { get; set; }
        
        public PermissionDO() { }

        public PermissionDO(DataRow dr)
        {
            if (dr == null) return;

            PermissionKey = dr.Table.Columns.Contains("PermissionKey")
                ? dr["PermissionKey"].ToSafeString()
                : string.Empty;

            IsReadOnly = dr.Table.Columns.Contains("IsReadOnlyPermission")
                ? dr["IsReadOnlyPermission"].ToSafeBool()
                : false;
        }
    }

}