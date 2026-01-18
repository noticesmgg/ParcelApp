using ParcelService.Services.Register;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Login
{
    public class LoginService : Service
    {
        public ILoginDataProvider _provider {  get; set; }

        public LoginResponse Post(LoginRequest loginRequest)
        {
            Console.WriteLine($"Post LoginRequest request started at {DateTime.Now:HH:mm:ss.fff}");
            var Response = _provider.Post(loginRequest);
            Console.WriteLine($"Post LoginRequest request completed at {DateTime.Now:HH:mm:ss.fff}");
            return Response;
        }

    }
}
