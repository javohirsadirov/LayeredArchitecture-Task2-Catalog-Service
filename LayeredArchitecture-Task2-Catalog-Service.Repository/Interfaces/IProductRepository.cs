using CatalogService.Repository.Models;

namespace CatalogService.Business.Interfaces;

public interface IProductRepository
{
    Task<Product> GetById(int id);
    Task<List<Product>> GetList(int? categoryId = null, int page = 1, int pageSize = 10);
    Task Create(Product product);
    Task<bool> Update(Product product);
    Task Delete(int id);
}
