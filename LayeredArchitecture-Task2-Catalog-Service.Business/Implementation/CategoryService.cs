using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;

internal class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task Create(CategoryDto categoryDto)
    {
        await categoryRepository.Create(new Category
        {
            Id = categoryDto.Id,
            ImageURL = categoryDto.ImageURL,
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCategoryId
        });
    }

    public async Task Delete(int id)
    {
        await categoryRepository.Delete(id);
    }

    public async Task<CategoryDto> GetById(int id)
    {
        var category = await categoryRepository.GetById(id);
        return new CategoryDto
        {
            Id = category.Id,
            ImageURL = category.ImageURL,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId
        };
    }

    public async Task<List<CategoryDto>> GetList()
    {
        var categories = await categoryRepository.GetList();
        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            ImageURL = category.ImageURL,
            Name = category.Name,
            ParentCategoryId = category.ParentCategoryId
        }).ToList();
    }

    public async Task Update(CategoryDto categoryDto)
    {
        await categoryRepository.Update(new Category
        {
            Id = categoryDto.Id,
            ImageURL = categoryDto.ImageURL,
            Name = categoryDto.Name,
            ParentCategoryId = categoryDto.ParentCategoryId
        });
    }
}