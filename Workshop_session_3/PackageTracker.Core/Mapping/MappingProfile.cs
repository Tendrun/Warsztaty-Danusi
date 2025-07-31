using AutoMapper;
using PackageTracker.Core.DTOs.User;
using PackageTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageTracker.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}
