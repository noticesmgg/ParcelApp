using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.TrueRecon.DataObjects
{
    public enum ReconStatus { Success, Failed, Running, Pending }

    [Route("/trigger", "GET")]
    public class InvokerDO : IReturn<InvokerResponseDO>
    {
        public int Id { get; set; }
        public long TriggerId { get; set; }
        public string ReconName { get; set; }
        public DateTime ReconDate { get; set; }
    }

    public class InvokerResponseDO
    {
        public int Id { get; set; }
        public long TriggerId { get; set; }
        public string ReconName { get; set; }
        public DateTime ReconDate { get; set; }
        public ReconStatus Status { get; set; }
        public string Message { get; set; } = "";
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
