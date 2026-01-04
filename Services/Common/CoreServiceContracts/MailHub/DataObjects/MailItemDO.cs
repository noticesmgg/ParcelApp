using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.MailHub.DataObjects
{
    //[Authenticate]
    [Route("/Send", "POST")]
    public class MailItemDO : IReturn<MailResponseDO>
    {
        [Required]
        [Input(Type = "textarea", Placeholder = "Enter email addresses separated by commas (e.g:  user1@example.com, user2@example.com)", Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+(,\s*[^@\s]+@[^@\s]+\.[^@\s]+)*$")]
        public List<string>? To { get; set; }
        [Input(Type = "textarea", Placeholder = "Enter CC addresses separated by commas (e.g., user1@example.com, user2@example.com)", Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+(,\s*[^@\s]+@[^@\s]+\.[^@\s]+)*$")]
        public List<string>? Cc { get; set; } = null;
        [Input(Type = "textarea", Placeholder = "Enter BCC addresses separated by commas (e.g., user1@example.com, user2@example.com)", Pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+(,\s*[^@\s]+@[^@\s]+\.[^@\s]+)*$")]
        public List<string>? Bcc { get; set; } = null;
        public string? Subject { get; set; }
        [Input(Type = "textarea")]
        public string? Body { get; set; }
        [Input(Type = "textarea", Placeholder = "Enter file paths or URLs separated by commas")]
        public List<string>? Attachments { get; set; } = null;
        public Dictionary<string, string>? FileAttachmentMap { get; set; }
        public bool Critical { get; set; } = false;
        public int JobId { get; set; } = 0;
        public string? JobDesc { get; set; }
        public string? MachineName { get; set; } = Environment.MachineName;
        public bool isActionRequired { get; set; } = false;
        public int? Id { get; set; } = null;
    }

    public class MailResponseDO
    {
        public bool Sent { get; set; }
        public string? Message { get; set; }
    }
}
