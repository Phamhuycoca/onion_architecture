using Application.Helpers;
using AutoMapper;
using MediatR;
using onion_architecture.Application.Features.Dto.UserDto;
using onion_architecture.Application.Helper;
using onion_architecture.Application.IService;
using onion_architecture.Application.Wrappers.Abstract;
using onion_architecture.Application.Wrappers.Concrete;
using onion_architecture.Domain.Entity;
using onion_architecture.Domain.Repositories;
using onion_architecture.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Service
{
    public class UserService : IUserService, IRequest<IResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public DataResponse<User> Create(CreateUser user)
        {
            string password = "12345678a";
            var dto = _mapper.Map<User>(user);
            dto.PassWord = PasswordHelper.CreateHashedPassword(password);
            var res = _userRepository.Create(dto);
            if (res != null)
            {
                return new DataResponse<User>(res, 200, "Success");
            }
            throw new ApiException(400, "Lỗi không thể tạo mới");
        }

        public DataResponse<User> Delete(long id)
        {
            var user = _userRepository.Delete(id);
            if (user != null)
            {
                return new DataResponse<User>(user, 200, "Success");
            }
            throw new ApiException(400, "Lỗi không thể xóa");
        }

        public DataResponse<UserDto> GetById(long id)
        {
            var user = _userRepository.GetById(id);
            var dto = _mapper.Map<UserDto>(user);

            if (user != null)
            {
                return new DataResponse<UserDto>(dto, 200, "Success");
            }
            throw new ApiException(400, "Lỗi không thể xóa");
        }

        public PagedDataResponse<UserDto> Items(int page, int pageSize, string? search)
        {
            var query = _mapper.Map<List<UserDto>>(_userRepository.GetAll());
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FullName.Contains(search)).ToList();
            }
            var paginatedResult = PaginatedList<UserDto>.ToPageList(query, page, pageSize);
            return new PagedDataResponse<UserDto>(paginatedResult, 200, paginatedResult.Count());
        }

        public DataResponse<User> Update(long id, UpdateUser user)
         {
            var entity = _userRepository.GetById(id);
            if (entity != null)
            {
                var res = _userRepository.Update(_mapper.Map<User>(user));
                if (res != null)
                {
                    return new DataResponse<User>(res, 200, "Success");
                }
            }
           
            throw new ApiException(400, "Lỗi không thể xóa");
        }
    }
}
