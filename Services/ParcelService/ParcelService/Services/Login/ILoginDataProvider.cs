using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Login
{
    public interface ILoginDataProvider
    {
        LoginResponse Post(LoginRequest loginRequest);
    }
}
