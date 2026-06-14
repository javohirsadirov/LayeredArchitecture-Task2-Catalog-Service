using CatalogService.Repository.Models;

namespace CatalogService.Business.Interfaces;

public interface ICategoryRepository
{
    Task<Category> GetById(int id);
    Task<List<Category>> GetList();
    Task Create(Category category);
    Task Update(Category category);
    Task Delete(int id);
}
