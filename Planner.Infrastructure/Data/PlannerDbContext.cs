using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;
using System.Reflection;
using System.Reflection.Emit;

namespace Planner.Infrastructure.Data
{
    public class PlannerDbContext:DbContext
    {
        public PlannerDbContext(DbContextOptions<PlannerDbContext> options):base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ApplySoftDeleteQueryFilter(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        private void ApplySoftDeleteQueryFilter(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var property = System.Linq.Expressions.Expression.Property(parameter, "IsDeleted");
                    var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                    var comparison = System.Linq.Expressions.Expression.Equal(property, falseConstant);
                    var lambda = System.Linq.Expressions.Expression.Lambda(comparison, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
