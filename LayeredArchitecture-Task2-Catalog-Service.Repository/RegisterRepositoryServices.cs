using CatalogService.Business.Implementation;
using CatalogService.Business.Interfaces;
using CatalogService.Repository.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Repository;

public static class RegisterRepositoryServices
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CatalogDb")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}
