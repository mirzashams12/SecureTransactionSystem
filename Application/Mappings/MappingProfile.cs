using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<UserCreateDto, User>();
        CreateMap<UserAuthenticateDto, User>();

        CreateMap<RefreshToken, RefreshTokenDto>();
    }
}