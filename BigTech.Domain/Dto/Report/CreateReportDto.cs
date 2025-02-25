namespace BigTech.Domain.Dto.Report;
public record CreateReportDto(string Name, string Description, long UserId)
{
    public string Name { get; set; }

    public string Description { get; set; }

    public long UserId { get; set; }
}
