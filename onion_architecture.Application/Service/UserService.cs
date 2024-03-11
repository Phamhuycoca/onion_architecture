using AutoMapper;
using onion_architecture.Application.Features.Dto.UserDto;
using onion_architecture.Application.IService;
using onion_architecture.Domain.Entity;
using onion_architecture.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public bool Create(CreateUser user)
        {
            return _userRepository.Create(_mapper.Map<User>(user)); 
        }

        public bool Delete(long id)
        {
            return _userRepository.Delete(id);
        }

        public List<UserDto> GetUsers()
        {
            return _mapper.Map<List<UserDto>>(_userRepository.GetAll());
        }

    }
}
