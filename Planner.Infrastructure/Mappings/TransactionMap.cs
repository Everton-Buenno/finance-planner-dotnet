using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.Entities;


namespace Planner.Infrastructure.Mappings
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.IsPaid)
                .HasDefaultValue(false);

            builder.Property(t => t.Ignored)
                .HasDefaultValue(false);

            builder.Property(t => t.Value)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(a => a.Account)
                .WithMany(t => t.Transactions)
                .HasForeignKey(a => a.AccountId)
                .HasConstraintName("FK_Transaction_Account");

            builder.HasOne(c => c.Category)
                .WithMany(t => t.Transactions)
                .HasForeignKey(c => c.CategoryId)
                .HasConstraintName("FK_Transaction_Category");

            builder.HasOne(t => t.CreditCard)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CreditCardId)
                .HasConstraintName("FK_Transaction_CreditCard")
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(t => t.Transactions)
             .WithOne(t => t.TransactionOrigem)
             .HasForeignKey(t => t.TransactionOrigemId)
             .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
