using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;

public interface ICategoryRepository
{
    Task<Category> GetById(int id);
    Task<List<Category>> GetList();
    Task Create(Category category);
    Task Update(Category category);
    Task Delete(int id);
}