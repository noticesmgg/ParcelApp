using System.Runtime.Serialization;

namespace CoreServiceContracts.ActionHub.DataObjects
{
    public class ProcessorTypeDO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [IgnoreDataMember]
        public string? Path { get; set; }
        public string HostName { get; set; } = "";
        [IgnoreDataMember]
        public string? Key { get { return $"{Id}:{Name}:{HostName}"; } }
    }
}
