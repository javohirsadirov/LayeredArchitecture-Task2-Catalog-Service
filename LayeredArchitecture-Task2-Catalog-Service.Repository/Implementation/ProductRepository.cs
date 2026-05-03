using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Data;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;

internal class ProductRepository(CatalogDbContext context) : IProductRepository
{
    public async Task Create(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var product = await context.Products.FindAsync((long)id)
            ?? throw new KeyNotFoundException($"Product with id {id} not found.");
        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }

    public async Task<Product> GetById(int id)
    {
        return await context.Products.FindAsync((long)id)
            ?? throw new KeyNotFoundException($"Product with id {id} not found.");
    }

    public async Task<List<Product>> GetList(int? categoryId = null, int page = 1, int pageSize = 10)
    {
        var query = context.Products.AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task Update(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }
}