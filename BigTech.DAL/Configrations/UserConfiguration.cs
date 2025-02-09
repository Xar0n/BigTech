using BigTech.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigTech.DAL.Configrations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();
        builder.Property(u => u.Login)
            .IsRequired(true)
            .HasMaxLength(100);
        builder.Property(u => u.Password)
            .IsRequired(true);

        builder.HasMany(u => u.Reports)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .HasPrincipalKey(u => u.Id);

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                u => u.HasOne<Role>()
                    .WithMany().HasForeignKey(r => r.RoleId),
                r => r.HasOne<User>()
                    .WithMany().HasForeignKey(u => u.UserId)
            );

        builder.HasData(new List<User>()
        {
            new User()
            {
                Id = 1,
                Login = "tony",
                Password = "5e-88-48-98-da-28-04-71-51-d0-e5-6f-8d-c6-29-27-73-60-3d-0d-6a-ab-bd-d6-2a-11-ef-72-1d-15-42-d8",
                CreatedAt = DateTime.UtcNow
            }
        });
    }
}
