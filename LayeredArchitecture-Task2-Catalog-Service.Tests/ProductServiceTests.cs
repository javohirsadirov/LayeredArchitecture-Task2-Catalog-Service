using LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;
using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Product;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;
using Moq;

namespace LayeredArchitecture_Task2_Catalog_Service.Tests;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductRepository> _productRepositoryMock;
    private IProductService _productService;

    [SetUp]
    public void SetUp()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }

    [Test]
    public async Task GetById_ReturnsCorrectProductDto()
    {
        var product = new Product
        {
            Id = 1, Name = "Laptop", Description = "Gaming laptop",
            ImageURL = "laptop.png", CategoryId = 1, Price = 999.99m, Amount = 10
        };
        _productRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(product);

        var result = await _productService.GetById(1);

        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Laptop"));
        Assert.That(result.Description, Is.EqualTo("Gaming laptop"));
        Assert.That(result.Price, Is.EqualTo(999.99m));
        Assert.That(result.Amount, Is.EqualTo(10));
        Assert.That(result.CategoryId, Is.EqualTo(1));
    }

    [Test]
    public async Task GetList_ReturnsAllProducts()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", CategoryId = 1, Price = 999.99m, Amount = 5 },
            new() { Id = 2, Name = "Mouse", CategoryId = 1, Price = 29.99m, Amount = 100 }
        };
        _productRepositoryMock.Setup(r => r.GetList()).ReturnsAsync(products);

        var result = await _productService.GetList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Laptop"));
        Assert.That(result[1].Name, Is.EqualTo("Mouse"));
    }

    [Test]
    public async Task Create_CallsRepositoryCreate()
    {
        var dto = new ProductDto
        {
            Name = "Keyboard", CategoryId = 1, Price = 49.99m, Amount = 50
        };

        await _productService.Create(dto);

        _productRepositoryMock.Verify(r => r.Create(It.Is<Product>(p =>
            p.Name == "Keyboard" && p.Price == 49.99m && p.Amount == 50)), Times.Once);
    }

    [Test]
    public async Task Update_CallsRepositoryUpdate()
    {
        var dto = new ProductDto
        {
            Id = 1, Name = "Updated Laptop", CategoryId = 2, Price = 1099.99m, Amount = 8
        };

        await _productService.Update(dto);

        _productRepositoryMock.Verify(r => r.Update(It.Is<Product>(p =>
            p.Id == 1 && p.Name == "Updated Laptop" && p.CategoryId == 2)), Times.Once);
    }

    [Test]
    public async Task Delete_CallsRepositoryDelete()
    {
        await _productService.Delete(1);

        _productRepositoryMock.Verify(r => r.Delete(1), Times.Once);
    }
}
