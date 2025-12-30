using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner.Infrastructure.Mappings
{
    public class BankAccountMap : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {

            builder.HasKey(a => a.Id);
            builder.Property(a => a.BankId)
                .IsRequired();

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.Color)
                .IsRequired()
                .HasMaxLength(7);

            builder.Property(a => a.Type)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(a => a.InitialBalance)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            builder.HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId);
        }
    }
}
