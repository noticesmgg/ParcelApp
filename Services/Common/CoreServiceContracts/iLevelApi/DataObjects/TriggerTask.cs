using CoreServiceContracts.ActionHub.DataObjects;
using ServiceStack;

namespace CoreServiceContracts.iLevelApi.DataObjects
{
    public enum TaskStatus { Success, Failed, Running, Pending, UnKnown }
    [Route("/trigger", "GET")]
    public class TriggerItemDO : IReturn<TaskResponseDO>
    {
        public int BorrowerId { get; set; }
        public DateTime? AsOfDate { get; set; } 
        public bool ReloadMetaData { get; set; } = false;
        public long TriggerId { get; set; }
    }

    [Route("/trigger/status", "GET")]
    public class TaskTriggerStatusDO : IReturn<TaskResponseDO>
    {
        public long TriggerId { get; set; }
    }

    public class TaskResponseDO
    {
        public int BorrowerId { get; set; }
        public long TriggerId { get; set; }
        public DateTime? AsOfDate { get; set; }
        public bool ReloadMetaData { get; set; } = false;
        public TaskStatus Status { get; set; }
        public string Message { get; set; } = "";
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    public class ResponseDO
    {
        public long TriggerId { get; set; }
        public TaskStatus Status { get; set; }
        public string Message { get; set; } = "";
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    [Route("/reloadAssets", "GET")]
    public class ReloadAssetsDO : IReturn<ResponseDO>
    {
        public long TriggerId { get; set; }
        public string UserName { get; set; } = "System";        
    }
}
