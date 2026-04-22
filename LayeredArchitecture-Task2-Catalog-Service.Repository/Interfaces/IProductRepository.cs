using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;

public interface IProductRepository
{
    Task<Product> GetById(int id);
    Task<List<Product>> GetList();
    Task Create(Product category);
    Task Update(Product category);
    Task Delete(int id);
}