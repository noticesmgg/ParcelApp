using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Register
{
    public class RegisterService : Service
    {
        public IRegisterDataProvider RegisterDataProvider {  get; set; }

        public RegisterResponse Post(RegisterRequest registerRequest)
        {
            Console.WriteLine($"Post RegisterRequest request started at {DateTime.Now:HH:mm:ss.fff}");
            var Response = RegisterDataProvider.Post(registerRequest);
            Console.WriteLine($"Post RegisterRequest request completed at {DateTime.Now:HH:mm:ss.fff}");
            return Response;
        }
    }
}
