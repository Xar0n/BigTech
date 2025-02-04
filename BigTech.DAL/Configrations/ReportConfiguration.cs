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
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(2000);
    }
}
