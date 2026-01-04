using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace CoreServiceContracts.DealCloud.DataObjects
{
    [Route("/PfoNotesUpdate", "GET POST")]
    public class PortfolioNotesUpdateRequestDO : IReturn<PortfolioNotesResponseDO>
    {
        public string UserName { get; set; } = string.Empty;
    }

    public class PortfolioNotesResponseDO
    {
        public Dictionary<String, String> Notes { get; set; } = new Dictionary<string, string>();
        public String ErrorMessage { get; set; } = String.Empty;
    }
}
