using onion_architecture.Application.Features.Auth;
using onion_architecture.Application.Wrappers.Concrete;
using onion_architecture.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.IService
{
    public interface IAuthService
    {
        DataResponse<TokenDTO> Login(LoginDto dto);
        TokenDTO CreateToken(User user);

    }
}
