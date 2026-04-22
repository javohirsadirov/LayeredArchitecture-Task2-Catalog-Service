using LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;
using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitecture_Task2_Catalog_Service.Business;

public static class RegisterBusinessServices
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}