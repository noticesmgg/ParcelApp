using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.Register
{
    public interface IRegisterDataProvider
    {
        RegisterResponse Post(RegisterRequest registerRequest);
    }
}
