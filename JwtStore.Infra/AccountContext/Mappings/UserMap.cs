using JwtStore.Core.AccountContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JwtStore.Infra.AccountContext.Mappings;

public class UserMap : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        
        builder.ToTable("User");
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name).HasColumnName("Name")
            .HasColumnType("NVARCHAR").HasMaxLength(100).IsRequired(true);
        builder.Property(u => u.Image).HasColumnName("Image")
            .HasColumnType("NVARCHAR").HasMaxLength(255).IsRequired(true);

        builder.OwnsOne(u => u.Email).Property(u => u.Address)
            .HasColumnName("Email").IsRequired(true);

        builder.OwnsOne(u => u.Email).OwnsOne(u => u.Verification)
            .Property(u => u.Code).HasColumnName("EmailVerificationCode")
            .IsRequired(true);

        builder.OwnsOne(u => u.Email).OwnsOne(u => u.Verification)
            .Property(u => u.ExpiresAt).HasColumnName("EmailExpiresAt")
            .IsRequired(false);

        builder.OwnsOne(u => u.Email).OwnsOne(u => u.Verification)
            .Property(u => u.VerifiedAt).HasColumnName("EmailVerifiedAt")
            .IsRequired(false);

        builder.OwnsOne(u => u.Email).OwnsOne(u => u.Verification)
            .Ignore(u => u.IsActive);

        builder.OwnsOne(e => e.Password).Property(e => e.Hash)
            .HasColumnName("PasswordHash").IsRequired();

        builder.OwnsOne(e => e.Password).Property(e => e.ResetCode)
            .HasColumnName("PasswordResetCode").IsRequired();
    }
}
