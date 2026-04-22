using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Product;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;

internal class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task Create(ProductDto productDto)
    {
        await productRepository.Create(new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId,
            Amount = productDto.Amount,
            ImageURL = productDto.ImageURL
        });
    }

    public async Task Delete(int id)
    {
        await productRepository.Delete(id);
    }

    public async Task<ProductDto> GetById(int id)
    {
        var product = await productRepository.GetById(id);
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Amount = product.Amount,
            ImageURL = product.ImageURL
        };
    }

    public async Task<List<ProductDto>> GetList()
    {
        var products = await productRepository.GetList();
        return products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Amount = product.Amount,
            ImageURL = product.ImageURL
        }).ToList();
    }

    public async Task Update(ProductDto productDto)
    {
        await productRepository.Update(new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId,
            Amount = productDto.Amount,
            ImageURL = productDto.ImageURL
        });
    }
}