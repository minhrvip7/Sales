using Sales.Application.IServices;
using Sales.Application.Mappings;
using Sales.Application.Services;
using Sales.Domain.IRepositories;
using Sales.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Sales.Api.Extensions
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
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUnitService, UnitService>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}

