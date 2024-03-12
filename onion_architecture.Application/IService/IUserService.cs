using onion_architecture.Application.Features.Dto.UserDto;
using onion_architecture.Application.Wrappers.Abstract;
using onion_architecture.Application.Wrappers.Concrete;
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
        PagedDataResponse<UserDto> Items(int page, int pageSize,string? search);
        DataResponse<User>Create(CreateUser user);
        DataResponse<User> Update(long id, UpdateUser user);
        DataResponse<User> Delete(long id);
        DataResponse<UserDto> GetById(long id);
    }
}
