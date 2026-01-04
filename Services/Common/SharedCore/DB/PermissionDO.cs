using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCore.DB
{
    /// <summary>
    /// Represents a user permission record (from usp_get_Perms).
    /// </summary>
    public class PermissionDO
    {
        public string PermissionKey { get; set; } = string.Empty;
        public bool IsReadOnly { get; set; }
    }
}
