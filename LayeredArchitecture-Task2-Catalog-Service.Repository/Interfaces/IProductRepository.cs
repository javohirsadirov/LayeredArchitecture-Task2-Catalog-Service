using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;

public interface IProductRepository
{
    Task<Product> GetById(int id);
    Task<List<Product>> GetList(int? categoryId = null, int page = 1, int pageSize = 10);
    Task Create(Product product);
    Task Update(Product product);
    Task Delete(int id);
}