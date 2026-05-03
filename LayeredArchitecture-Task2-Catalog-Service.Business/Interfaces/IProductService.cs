using LayeredArchitecture_Task2_Catalog_Service.Dtos.Product;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetById(int id);
    Task<List<ProductDto>> GetList(int? categoryId = null, int page = 1, int pageSize = 10);
    Task Create(ProductDto productDto);
    Task Update(ProductDto productDto);
    Task Delete(int id);
}