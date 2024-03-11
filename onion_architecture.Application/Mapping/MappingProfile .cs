using AutoMapper;
using onion_architecture.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using onion_architecture.Application.Features.Dto.UserDto;

namespace onion_architecture.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //User
            CreateMap<User, CreateUser>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();

        }
    }
}
