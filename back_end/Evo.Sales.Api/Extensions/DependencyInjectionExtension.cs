using Evo.Sales.Application.IServices;
using Evo.Sales.Application.Mappings;
using Evo.Sales.Application.Services;
using Evo.Sales.Domain.IRepositories;
using Evo.Sales.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Evo.Sales.Api.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Register Repositories and Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Register Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
