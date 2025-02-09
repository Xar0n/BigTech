using BigTech.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigTech.DAL.Configrations;
public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.Property(ut => ut.Id).ValueGeneratedOnAdd();
        builder.Property(ut => ut.RefreshToken).IsRequired(true);
        builder.Property(ut => ut.RefreshTokenExpiryTime).IsRequired(true);

        /*builder.HasData(new List<UserToken>()
        {
            new UserToken()
            {
                Id = 1,
                RefreshToken = "I03ig04ig094uig94ikfeg",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                UserId = 1
            }
        });*/
    }
}
