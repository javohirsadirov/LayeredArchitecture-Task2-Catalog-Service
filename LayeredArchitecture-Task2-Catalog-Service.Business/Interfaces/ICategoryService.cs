using CatalogService.Dtos.Category;

namespace CatalogService.Business.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto> GetById(int id);
    Task<List<CategoryDto>> GetList();
    Task Create(CategoryDto categoryDto);
    Task Update(CategoryDto categoryDto);
    Task Delete(int id);
}
