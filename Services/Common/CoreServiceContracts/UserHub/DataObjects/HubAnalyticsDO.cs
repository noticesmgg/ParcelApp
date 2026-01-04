using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.UserHub.DataObjects
{
    [Route("/HubAnalytics", "POST")]
    public class HubAnalyticsDO : IReturn<HubAnalyticsDO>
    {
        public long SessionId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string MachineName { get; set; }
        public string Version { get; set; }
        public DateTime LoggedInAt { get; set; }
        public List<string> Screens { get; set; }
        public DateTime LastHeartBeat { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool Error { get; set; }
        public string LogFile { get; set; }
    }

    [Route("/GetActivity", "GET")]
    public class GetUserActivity : IReturn<List<HubAnalyticsDO>> { }

    public class UserResponse
    {
        public bool Success { get; set; }
    }
}
