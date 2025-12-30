using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Planner.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Planner.Domain.Interfaces;
using Planner.Infrastructure.Repositories;

namespace Planner.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration)
                    .AddRepositories();
            return services;
        }
        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PlannerDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsAssembly(typeof(PlannerDbContext).Assembly.FullName);

                        npgsqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,                  
                            maxRetryDelay: TimeSpan.FromSeconds(5), 
                            errorCodesToAdd: null                
                        );
                    });
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();
            return services;
        }
    }
}
