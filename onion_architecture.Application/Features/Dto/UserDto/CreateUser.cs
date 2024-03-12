using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Features.Dto.UserDto
{
    public class CreateUser
    {
        public long UserId { get; set; }
        public string? FullName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public string? Role { get; set; }
    }
}
