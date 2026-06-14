using CatalogService.Business.Implementation;
using CatalogService.Business.Interfaces;
using CatalogService.Dtos.Product;
using CatalogService.MessageQueue;
using CatalogService.MessageQueue.Interfaces;
using CatalogService.Repository.Models;

using Microsoft.Extensions.Options;

using Moq;

namespace CatalogService.Tests;

/// <summary>
/// Unit tests for the product service.
/// </summary>
[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductRepository> productRepositoryMock;
    private Mock<IMessagePublisher> messagePublisherMock;
    private IProductService productService;

    /// <summary>
    /// Initializes test dependencies before each test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        productRepositoryMock = new Mock<IProductRepository>();
        messagePublisherMock = new Mock<IMessagePublisher>();
        var rabbitOptions = Options.Create(new RabbitMQOptions
        {
            ProductUpdated = new ProductUpdatedSettings
            {
                Exchange = "catalog.exchange",
                Queue = "cart.update.queue",
                RoutingKey = "catalog.item.updated"
            }
        });
        productService = new ProductService(productRepositoryMock.Object, messagePublisherMock.Object, rabbitOptions);
    }

    /// <summary>
    /// Verifies that GetById returns the correct product DTO.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetByIdReturnsCorrectProductDto()
    {
        var product = new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Gaming laptop",
            ImageURL = "laptop.png",
            CategoryId = 1,
            Price = 999.99m,
            Amount = 10
        };
        productRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(product);

        var result = await productService.GetById(1);

        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Laptop"));
        Assert.That(result.Description, Is.EqualTo("Gaming laptop"));
        Assert.That(result.Price, Is.EqualTo(999.99m));
        Assert.That(result.Amount, Is.EqualTo(10));
        Assert.That(result.CategoryId, Is.EqualTo(1));
    }

    /// <summary>
    /// Verifies that GetList returns all products.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetListReturnsAllProducts()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", CategoryId = 1, Price = 999.99m, Amount = 5 },
            new() { Id = 2, Name = "Mouse", CategoryId = 1, Price = 29.99m, Amount = 100 }
        };
        productRepositoryMock.Setup(r => r.GetList(It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(products);

        var result = await productService.GetList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Laptop"));
        Assert.That(result[1].Name, Is.EqualTo("Mouse"));
    }

    /// <summary>
    /// Verifies that Create calls the repository Create method.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task CreateCallsRepositoryCreate()
    {
        var dto = new CreateProductDto
        {
            Name = "Keyboard",
            CategoryId = 1,
            Price = 49.99m,
            Amount = 50
        };

        await productService.Create(dto);

        productRepositoryMock.Verify(r => r.Create(It.Is<Product>(p =>
            p.Name == "Keyboard" && p.Price == 49.99m && p.Amount == 50)), Times.Once);
    }

    /// <summary>
    /// Verifies that Update calls the repository Update method.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task UpdateCallsRepositoryUpdate()
    {
        var dto = new ProductDto
        {
            Id = 1,
            Name = "Updated Laptop",
            CategoryId = 2,
            Price = 1099.99m,
            Amount = 8
        };

        await productService.Update(dto);

        productRepositoryMock.Verify(r => r.Update(It.Is<Product>(p =>
            p.Id == 1 && p.Name == "Updated Laptop" && p.CategoryId == 2)), Times.Once);
    }

    /// <summary>
    /// Verifies that Delete calls the repository Delete method.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task DeleteCallsRepositoryDelete()
    {
        await productService.Delete(1);

        productRepositoryMock.Verify(r => r.Delete(1), Times.Once);
    }
}
