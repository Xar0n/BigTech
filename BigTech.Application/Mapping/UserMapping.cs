using AutoMapper;
using BigTech.Domain.Dto.User;
using BigTech.Domain.Entity;

namespace BigTech.Application.Mapping;
public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
