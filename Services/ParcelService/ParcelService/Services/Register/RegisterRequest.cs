using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Register
{
     [Route("/auth/register", "POST")]
    public class RegisterRequest : IReturn<bool>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class RegisterResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }

    }
}
