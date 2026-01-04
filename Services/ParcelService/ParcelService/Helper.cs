using SharedCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService
{
    internal class Helper
    {
        public static string? ServiceStackLicense { get; set; }
            = "22980-e3JlZjoyMjk4MCxuYW1lOk1HRyBJbnZlc3RtZW50IEdyb3VwIExQLHR5cGU6QnVzaW5lc3MsbWV0YTowLGhhc2g6dHVoUVhGRjc5WDhoUmtLNFpWUlNwZGVZZXFPS1BKdm91dEFkbHJNUWRGQkdsdks5Yk5nd1J0UUk1QUw1amdwU21KcEU2cXUwUGlnUEs0SGJjc0hkajhxc0xqSlFaeWppUEhTNkV1bFpHcFF2R1kzRjlLY1kzdFFYbTNvbklZNjdxUUtFUGVjTDZsWmNUQUJXUjNWL2k5Zm9UVmFjTzMwUEcvdjF5eHBBM1NnPSxleHBpcnk6MjAyNi0wMS0wNn0=";
        public static int PortNumber { get; set; } = 9867;
        public static string HostName { private set; get; } = Environment.MachineName;
        public static bool Shutdown { get; set; } = false;
        private static long lastId = DateTime.Now.ToString("yyyyMMddhhmmss").ToSafeLong();
        public static bool IsRunning
        {
            get
            {
                return !Shutdown;
            }
        }
    }
}
