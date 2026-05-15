using Auth.Api.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Api.DAL.Configurations
{
    public sealed class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.ToTable("Accounts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                   .HasMaxLength(512);

            builder.Property(x => x.Email)
                   .HasMaxLength(256);

            builder.Property(x => x.ExternalId)
                   .HasMaxLength(256);

            builder.Property(x => x.JwtRefreshToken)
                   .HasMaxLength(128);

            builder.Property(x => x.AuthType)
                   .IsRequired();

            builder.Property(x => x.Role)
                   .IsRequired();

            builder.Property(x => x.CreatedUtcDate)
                .IsRequired();

            builder.Property(x => x.UpdateUtcDate)
                .IsRequired();

            builder.HasIndex(x => x.JwtRefreshToken)
                   .IsUnique();

            builder.HasIndex(x => x.ExternalId)
                   .IsUnique();
        }
    }
}
