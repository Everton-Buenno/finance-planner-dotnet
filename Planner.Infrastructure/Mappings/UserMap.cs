using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.Entities;
using Planner.Domain.ValueObjects;

namespace Planner.Domain.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email)
                .HasConversion(
                email => email.Value,
                value => new Email(value)
                )
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(u => u.Cpf)
                    .HasConversion(
                        cpf => cpf.Value,
                        value => new Cpf(value))
                    .HasMaxLength(11);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Plan)
                .IsRequired();

            builder.Property(u => u.Gender)
              .HasConversion<int>();

            builder.Property(u => u.Nationality)
                .HasMaxLength(50)
                ;

            builder.Property(u => u.DarkMode)
                .IsRequired();

            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street).HasMaxLength(100);
                address.Property(a => a.Number).HasMaxLength(20);
                address.Property(a => a.Complement).HasMaxLength(100);
                address.Property(a => a.City).HasMaxLength(100);
                address.Property(a => a.State).HasMaxLength(50);
                address.Property(a => a.ZipCode).HasMaxLength(10);
                address.Property(a => a.Country).HasMaxLength(50);
            });

            builder.OwnsOne(u => u.PhoneNumber, phone =>
            {
                phone.Property(p => p.CountryCode).HasMaxLength(5);
                phone.Property(p => p.Number).HasMaxLength(15);
            });

            builder.HasIndex(u => u.Email)
              .IsUnique();
            
            builder.HasIndex(u => u.Cpf)
            .IsUnique();

        }
    }
}
