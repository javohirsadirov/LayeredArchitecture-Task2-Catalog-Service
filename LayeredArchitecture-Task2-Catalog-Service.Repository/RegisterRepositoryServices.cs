using LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;
using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitecture_Task2_Catalog_Service.Repository;

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