using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Features.Auth
{
    public class LoginDto
    {
        public string? Email { get; set; }
        public string? PassWord { get; set; }
    }
}
