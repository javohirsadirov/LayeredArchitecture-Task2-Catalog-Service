using LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Data;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace LayeredArchitecture_Task2_Catalog_Service.Tests;

[TestFixture]
public class ProductRepositoryIntegrationTests
{
    private CatalogDbContext _context;
    private ProductRepository _productRepository;
    private CategoryRepository _categoryRepository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer("Server=localhost;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        _context = new CatalogDbContext(options);
        _context.Database.EnsureCreated();
        _productRepository = new ProductRepository(_context);
        _categoryRepository = new CategoryRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private async Task<Category> CreateTestCategory()
    {
        var category = new Category { Name = "TestCategory" };
        await _categoryRepository.Create(category);
        return category;
    }

    [Test]
    public async Task Create_And_GetById_ReturnsCreatedProduct()
    {
        var category = await CreateTestCategory();
        var product = new Product
        {
            Name = "Laptop", Description = "Gaming laptop",
            CategoryId = category.Id, Price = 999.99m, Amount = 10
        };

        await _productRepository.Create(product);
        var result = await _productRepository.GetById((int)product.Id);

        Assert.That(result.Name, Is.EqualTo("Laptop"));
        Assert.That(result.Price, Is.EqualTo(999.99m));
        Assert.That(result.CategoryId, Is.EqualTo(category.Id));
    }

    [Test]
    public async Task Update_ModifiesProduct()
    {
        var category = await CreateTestCategory();
        var product = new Product { Name = "Old", CategoryId = category.Id, Price = 10m, Amount = 1 };
        await _productRepository.Create(product);

        product.Name = "Updated";
        product.Price = 20m;
        await _productRepository.Update(product);

        var result = await _productRepository.GetById((int)product.Id);
        Assert.That(result.Name, Is.EqualTo("Updated"));
        Assert.That(result.Price, Is.EqualTo(20m));
    }

    [Test]
    public async Task Delete_RemovesProduct()
    {
        var category = await CreateTestCategory();
        var product = new Product { Name = "ToDelete", CategoryId = category.Id, Price = 5m, Amount = 1 };
        await _productRepository.Create(product);

        await _productRepository.Delete((int)product.Id);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _productRepository.GetById((int)product.Id));
    }
}
