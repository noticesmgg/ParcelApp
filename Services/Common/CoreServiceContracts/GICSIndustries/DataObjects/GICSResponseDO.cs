using ServiceStack;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Script.Lisp;

namespace CoreServiceContracts.DealCloud.DataObjects
{
    public class GICSResponseDO
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public string? Sector { get; set; }
        public int SubIndustryCode { get; set; }
        public string? SubIndustry { get; set; }
        public string? SubIndustryDefinition { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? IndustryGroup { get; set; }
        public string? Industry { get; set; }

        public GICSResponseDO()
        {
        }
    }
}
