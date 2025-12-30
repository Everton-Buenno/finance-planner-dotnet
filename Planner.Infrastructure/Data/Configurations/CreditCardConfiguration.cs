using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.Entities;

namespace Planner.Infrastructure.Data.Configurations
{
    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder.ToTable("CreditCards");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CreditLimit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.DueDay)
                .IsRequired();

            builder.Property(c => c.ClosingDay)
                .IsRequired();

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.Property(c => c.UpdatedAt);

            builder.Property(c => c.DeletedAt);

            builder.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(c => c.Account)
                .WithMany(a => a.CreditCards)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Transactions)
                .WithOne()
                .HasForeignKey("CreditCardId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.AccountId);
            builder.HasIndex(c => new { c.AccountId, c.Description }).IsUnique();
            builder.HasIndex(c => c.IsActive);
            builder.HasIndex(c => c.IsDeleted);
        }
    }
}