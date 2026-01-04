using ServiceStack;


namespace CoreServiceContracts.HeartbeatApi.DataObjects
{
    [Route("/fileStatus", "GET POST")]
    public class FileStatus : IReturn<FileStatusResponseDO>
    {
        public int FileId { get; set; }
        public int ProcessId { get; set; }
        public DateTime? AsOfDate { get; set; } = DateTime.Now;
        public string ActualFileName { get; set; } = "";
        public Status Status { get; set; }
        public string Message { get; set; } = "";
        public long TriggerId { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
    
    public class FileStatusResponseDO
    {
        public int JobId { get; set; }
        public int ProcessId { get; set; }
        public int FileId { get; set; }
        public String ProcessName { get; set; } = "";
        public DateTime? AsOfDate { get; set; }
        public string FileMask { get; set; } = "";
        public string ActualFileName { get; set; } = "";
        public string DestinationPath { get; set; }
        public DateTime LastUpdated { get; set; }
        public Status Status { get; set; } 
        public bool Updated { get; set; } = false;
        public string Message { get; set; } = "";
        public long TriggerId { get; set; }
    }
}
