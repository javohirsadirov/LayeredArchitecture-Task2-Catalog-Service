using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> GetById(int id);
    Task<List<CategoryDto>> GetList();
    Task Create(CategoryDto categoryDto);
    Task Update(CategoryDto categoryDto);
    Task Delete(int id);
}