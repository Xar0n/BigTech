using AutoMapper;
using BigTech.Domain.Dto.Report;
using BigTech.Domain.Entity;

namespace BigTech.Application.Mapping;
public class ReportMapping : Profile
{
    public ReportMapping()
    {
        CreateMap<Report, ReportDto>().ReverseMap();
    }
}
