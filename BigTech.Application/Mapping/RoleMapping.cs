using AutoMapper;
using BigTech.Domain.Dto.Role;
using BigTech.Domain.Entity;

namespace BigTech.Application.Mapping;
public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}
