using AutoMapper;
using PackageTracker.Core.DTOs.Carrier;
using PackageTracker.Core.DTOs.PackageDTO;
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
            // User types
            CreateMap<CreateUserDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<UpdateUserDTO, User>();

            // Package types
            CreateMap<Package, PackageDto>();
            CreateMap<PackageDto, Package>();

            // Carrier types
            CreateMap<CreateCarrierDTO, CarrierDTO>();
            CreateMap<Carrier, UpdateCarrierDTO>();
            CreateMap<Carrier, CarrierDTO>();

        }
    }
}
