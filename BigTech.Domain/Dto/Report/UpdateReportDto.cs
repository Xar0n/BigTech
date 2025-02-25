namespace BigTech.Domain.Dto.Report;
public record UpdateReportDto(long Id, string Name, string Description)
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
