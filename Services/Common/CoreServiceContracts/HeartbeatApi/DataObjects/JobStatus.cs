using ServiceStack;

namespace CoreServiceContracts.HeartbeatApi.DataObjects
{
    public enum Status { NA, NotStarted, Running, Succeeded, Failed, Cancelled }
    public enum JobType { Report = 1, HeartBeat = 2, BatchJob = 3, Email = 4 }
    [Route("/jobStatus", "GET POST")]
    public class JobStatusDO : IReturn<JobStatusResponseDO>
    {
        public int JobId { get; set; }
        public string Name { get; set; } = "";
        public DateTime? AsOfDate { get; set; }
        public Status Status { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public long TriggerId { get; set; }
    }

    [Route("/jobs", "GET POST")]
    public class JobRequestDO : IReturn<JobStatusResponseDO[]>
    {
        public int JobId { get; set; }        
    }

    public class JobStatusResponseDO
    {
        public int JobId { get; set; }
        public string Name { get; set; } = "";
        public JobType JobType { get; set; }
        public DateTime? AsOfDate { get; set; }
        public DateTime? ExpectedTime { get; set; }
        public Status Status { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool Updated { get; set; } = false;
        public string Message { get; set; } = "";
        public long TriggerId { get; set; }
        public string RunsOn { get; set; }
    }
}
