using AutoMapper;
using Server.Models.Entities;
using Server.Models.Requests;

namespace Server.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserCreateRequest, User>();
    }
}
