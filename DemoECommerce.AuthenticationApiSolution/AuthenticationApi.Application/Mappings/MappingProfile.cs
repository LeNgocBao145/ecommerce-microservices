using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;
using AutoMapper;

namespace AuthenticationApi.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRequestDTO>();
            CreateMap<UserRequestDTO, User>();

            CreateMap<User, UserResponseDTO>();
            CreateMap<UserResponseDTO, User>();
        }
    }
}
