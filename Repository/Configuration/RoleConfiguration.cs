using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole()
            {
                Id = "58f6b12f-7a1c-42c9-b4e8-bd9e5000c729",
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new IdentityRole()
            {
                Id = "8e51f144-b859-4867-bed0-7dec288ef7b9",
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            });
    }
}
