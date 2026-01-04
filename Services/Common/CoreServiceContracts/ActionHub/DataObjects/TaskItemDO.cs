using ServiceStack;
using System.Runtime.Serialization;

namespace CoreServiceContracts.ActionHub.DataObjects
{
    //[Authenticate]
    [Route("/tasks", "GET")]
    public class TaskItem : IReturn<TaskItemDO[]>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class TaskItemDO
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public string? ProcessorName
        {
            get
            {
                if (ProcessorType != null)
                    return ProcessorType.Name;
                else
                    return "";
            }
        }

        public string? RunsOn
        {
            get
            {
                if (ProcessorType != null)
                    return ProcessorType.HostName;
                else
                    return HostName;
            }
        }
        public string TaskParams { get; set; } = "";
        public DateTime NextScheduledRun { get; set; }
        [IgnoreDataMember]
        public string HostName { get; set; } = "";
        public string CronSchedule { get; set; } = "";
        public string TimezoneInfo { get; set; } = "";
        public int JobId { get; set; }
        public DateTime LastModified { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastRun { get; set; }
        public bool IsCompleted { get; set; }
        public string? Error { get; set; }
        [IgnoreDataMember]
        public ProcessorTypeDO? ProcessorType { get; set; }
        [IgnoreDataMember]
        public int ProcessorTypeId { get; set; }
        [IgnoreDataMember]
        public string Key { get { return $"{Id}:{TaskName}:{HostName}"; } }



    }
}
