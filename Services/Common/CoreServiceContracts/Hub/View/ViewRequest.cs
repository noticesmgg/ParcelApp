using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.Hub.View
{
    [Route("/view", "GET POST")]
    public class ViewRequest
    {
        public int Type { get; set; }
        public string State { get; set; }
    }
}
