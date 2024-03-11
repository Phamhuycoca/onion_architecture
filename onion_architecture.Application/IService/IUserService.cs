using onion_architecture.Application.Features.Dto.UserDto;
using onion_architecture.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.IService
{
    public interface IUserService
    {
        List<UserDto> GetUsers();
        bool Create(CreateUser user);
        bool Delete(long ig);
    }
}
