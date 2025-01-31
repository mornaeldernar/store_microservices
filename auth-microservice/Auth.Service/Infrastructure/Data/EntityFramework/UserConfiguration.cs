using Auth.Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Service.Infrastructure.Data.EntityFrameworkCore;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired();

        builder.Property(u => u.Password)
            .IsRequired();
        builder.Property(u => u.Role)
            .IsRequired();
        builder.HasData(
            new User
            {
                Id = Guid.NewGuid(),
                Username = "jimenez.rafa@gmail.com",
                Password = "KoNrqkO7iC#G",
                Role = "Administrator"
            });
    }
}