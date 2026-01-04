using ServiceStack;

namespace CoreServiceContracts.ActionHub.DataObjects
{
    public enum TaskStatus { Success, Failed, Running, Pending }

   //[Authenticate]
    [Route("/trigger", "GET")]
    public class TriggerItemDO : IReturn<TriggerResponseDO>
    {
        public int Id { get; set; }
        public long TriggerId { get; set; }
    }

    //[Authenticate]
    [Route("/trigger/status", "GET")]
    public class TaskTriggerStatusDO : IReturn<TriggerResponseDO>
    {
        public long TriggerId { get; set; }
    }

    public class TriggerResponseDO
    {
        public int Id { get; set; }
        public long TriggerId { get; set; }
        public TaskStatus Status { get; set; }
        public string Message { get; set; } = "";
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

}
