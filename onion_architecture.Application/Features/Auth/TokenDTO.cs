using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Features.Auth
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public int AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
