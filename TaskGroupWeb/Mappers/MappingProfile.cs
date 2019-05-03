using AutoMapper;
using Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskGroupWeb.Models;

namespace TaskGroupWeb.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
        }
    }
}
