using identityservice.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sharedkernel;

namespace identityservice.Infrastructure.Persistence
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(x => x.Id);
            builder.Property(b => b.Name).IsRequired();
            builder.Property(b => b.IsDefault).IsRequired().HasDefaultValue<bool>(false);
            builder.Property(b => b.CreatedBy).IsRequired();
            builder.Property(b => b.CreatedDate).IsRequired();

            builder.Property(b => b.LastModifiedBy);
            builder.Property(b => b.LastModifiedDate);
            builder.Property(b => b.DeletedBy);
            builder.Property(b => b.DeletedDate);
            builder.Property(b => b.Status).IsRequired().HasDefaultValue<Status>(Status.Active);

            builder.HasMany(b => b.Users).WithOne();

        }

    }
}

