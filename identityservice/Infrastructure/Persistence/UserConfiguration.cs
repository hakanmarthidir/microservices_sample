using identityservice.Domain.UserAggregate;
using identityservice.Domain.UserAggregate.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sharedkernel;

namespace identityservice.Infrastructure.Persistence
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);

            builder
           .HasOne(p => p.Role)
           .WithMany(b => b.Users)
           .HasForeignKey(p => p.RoleId).IsRequired();

            builder.Property(b => b.CreatedBy).IsRequired();
            builder.Property(b => b.CreatedDate).IsRequired();

            builder.Property(b => b.LastModifiedBy);
            builder.Property(b => b.LastModifiedDate);
            builder.Property(b => b.DeletedBy);
            builder.Property(b => b.DeletedDate);
            builder.Property(b => b.Status).IsRequired().HasDefaultValue<Status>(Status.Active);

            //ValueType Settings
            //builder.OwnsOne(o => o.Activation);
            builder.OwnsOne(p => p.Activation)
                .Property(p => p.IsActivated)
                .IsRequired()
                .HasDefaultValue<ActivationStatusEnum>(ActivationStatusEnum.NotActivated);

            builder.OwnsOne(p => p.Activation).Property(p => p.ActivationCode).IsRequired();
            builder.OwnsOne(p => p.Activation).Property(p => p.ActivationDate);

            //builder.OwnsOne(o => o.FullName);
            builder.OwnsOne(p => p.FullName).Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.OwnsOne(p => p.FullName).Property(p => p.Surname).IsRequired().HasMaxLength(255);

            //builder.OwnsOne(o => o.EmailAddress);
            builder.OwnsOne(p => p.Email).Property(p => p.EmailAddress).IsRequired().HasMaxLength(255);
            //builder.HasIndex(x => x.Email.EmailAddress).IsUnique();

            //builder.OwnsOne(o => o.Password);
            builder.OwnsOne(p => p.Password).Property(p => p.Password).IsRequired().HasMaxLength(255);

            builder.HasMany(b => b.Tokens).WithOne(e => e.User);
        }

    }
}

