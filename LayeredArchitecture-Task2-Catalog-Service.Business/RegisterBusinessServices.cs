using CatalogService.Business.Implementation;
using CatalogService.Business.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Business;

public static class RegisterBusinessServices
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}
