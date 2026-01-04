using ServiceStack;
using System.Collections.Generic;

namespace CoreServiceContracts.Hub.Menu
{
    [Route("/menu", "GET POST")]
    public class MenuRequest : IReturn<MenuDO[]>
    {
        public string? UserName { get; set; }
    }
}
