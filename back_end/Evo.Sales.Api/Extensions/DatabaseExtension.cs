using Evo.Sales.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evo.Sales.Api.Extensions
{
    public static class DatabaseExtension
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<SalesDbContext>(options =>
                options.UseNpgsql(connectionString, b => 
                    b.MigrationsAssembly("Evo.Sales.Infrastructure")));

            return services;
        }
    }
}
