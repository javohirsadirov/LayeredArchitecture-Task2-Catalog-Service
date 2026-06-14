using CatalogService.Dtos.Product;

namespace CatalogService.Business.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetById(int id);
    Task<List<ProductDto>> GetList(int? categoryId = null, int page = 1, int pageSize = 10);
    Task<long> Create(CreateProductDto productDto);
    Task Update(ProductDto productDto);
    Task Delete(int id);
}
