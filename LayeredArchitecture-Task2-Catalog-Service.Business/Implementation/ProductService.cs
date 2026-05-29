using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Product;
using LayeredArchitecture_Task2_Catalog_Service.MessageQueue;
using LayeredArchitecture_Task2_Catalog_Service.MessageQueue.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;
using Microsoft.Extensions.Options;

namespace LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;

internal class ProductService(IProductRepository productRepository,
    IMessagePublisher messagePublisher,
    IOptions<RabbitMQOptions> rabbitMQOptions) : IProductService
{
    private readonly ProductUpdatedSettings _productUpdatedSettings = rabbitMQOptions.Value.ProductUpdated;
    public async Task<long> Create(CreateProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId,
            Amount = productDto.Amount,
            ImageURL = productDto.ImageURL
        };
        await productRepository.Create(product);
        return product.Id;
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

    public async Task<List<ProductDto>> GetList(int? categoryId = null, int page = 1, int pageSize = 10)
    {
        var products = await productRepository.GetList(categoryId, page, pageSize);
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
        var updated = await productRepository.Update(new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CategoryId = productDto.CategoryId,
            Amount = productDto.Amount,
            ImageURL = productDto.ImageURL
        });

        if (updated)
        {
            await messagePublisher.PublishAsync(
                _productUpdatedSettings.Exchange,
                _productUpdatedSettings.RoutingKey,
                new
                {
                    Event = "ProductUpdated",
                    productDto.Id,
                    productDto.Name,
                    productDto.Price,
                    productDto.CategoryId,
                    Timestamp = DateTime.UtcNow
                });
        }
    }
}