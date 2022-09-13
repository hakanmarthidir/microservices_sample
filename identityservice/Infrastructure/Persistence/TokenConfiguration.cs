using identityservice.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace identityservice.Infrastructure.Persistence
{
    public class TokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Token).IsRequired();
            builder.Property(b => b.IssuedAt).IsRequired();
            builder.Property(b => b.ExpiresAt).IsRequired();

            builder.HasOne(b => b.User)
                .WithMany(u => u.Tokens);
        }

    }
}

