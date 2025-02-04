namespace BigTech.Domain.Dto.Report;
public record class CreateReportDto
{
    public string Name { get; set; }

    public string Description { get; set; }

    public long UserId { get; set; }
}
