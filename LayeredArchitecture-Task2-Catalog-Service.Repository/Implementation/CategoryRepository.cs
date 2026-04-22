using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;

internal class CategoryRepository(CatalogDbContext context) : ICategoryRepository
{
    public async Task Create(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var category = await context.Categories.FindAsync((long)id)
            ?? throw new KeyNotFoundException($"Category with id {id} not found.");
        context.Categories.Remove(category);
        await context.SaveChangesAsync();
    }

    public async Task<Category> GetById(int id)
    {
        return await context.Categories.FindAsync((long)id)
            ?? throw new KeyNotFoundException($"Category with id {id} not found.");
    }

    public async Task<List<Category>> GetList()
    {
        return await context.Categories.ToListAsync();
    }

    public async Task Update(Category category)
    {
        context.Categories.Update(category);
        await context.SaveChangesAsync();
    }
}