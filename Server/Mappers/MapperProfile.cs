﻿using AutoMapper;
using Server.Models.Dtos;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;

namespace Server.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserCreateRequest, User>();
        CreateMap<UserLoginRequest, User>();
        CreateMap<UserLoginDto, UserLoginResponse>();
    }
}
