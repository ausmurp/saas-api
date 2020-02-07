using AutoMapper;
using SaaSApi.Data.Entities;
using SaaSApi.Logic.Models;

namespace SaaSApi.Logic
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}