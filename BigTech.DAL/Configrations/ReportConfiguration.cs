using BigTech.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigTech.DAL.Configrations;
public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(r => r.Id).ValueGeneratedOnAdd();
        builder.Property(r => r.Name)
            .IsRequired(true)
            .HasMaxLength(100);
        builder.Property(r => r.Description)
            .IsRequired(true)
            .HasMaxLength(2000);

        builder.HasData(new List<Report>()
        {
            new Report()
            {
                Id = 1,
                Name = "Report #1",
                Description = "Test description",
                UserId = 1,
                CreatedAt = DateTime.UtcNow
            }
        });
    }
}
