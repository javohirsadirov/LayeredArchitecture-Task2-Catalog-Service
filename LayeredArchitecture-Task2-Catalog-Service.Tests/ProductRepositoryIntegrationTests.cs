using CatalogService.Business.Implementation;
using CatalogService.Repository.Data;
using CatalogService.Repository.Models;

using Microsoft.EntityFrameworkCore;

namespace CatalogService.Tests;

/// <summary>
/// Integration tests for the product repository.
/// </summary>
[TestFixture]
public class ProductRepositoryIntegrationTests
{
    private CatalogDbContext context;
    private ProductRepository productRepository;
    private CategoryRepository categoryRepository;

    /// <summary>
    /// Initializes the database context and repositories before each test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer("Server=localhost;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        context = new CatalogDbContext(options);
        context.Database.EnsureCreated();
        productRepository = new ProductRepository(context);
        categoryRepository = new CategoryRepository(context);
    }

    /// <summary>
    /// Disposes the database context after each test.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    private async Task<Category> CreateTestCategory()
    {
        var category = new Category { Name = "TestCategory" };
        await categoryRepository.Create(category);
        return category;
    }

    /// <summary>
    /// Verifies that a created product can be retrieved by its identifier.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task CreateAndGetByIdReturnsCreatedProduct()
    {
        var category = await CreateTestCategory();
        var product = new Product
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            CategoryId = category.Id,
            Price = 999.99m,
            Amount = 10
        };

        await productRepository.Create(product);
        var result = await productRepository.GetById((int)product.Id);

        Assert.That(result.Name, Is.EqualTo("Laptop"));
        Assert.That(result.Price, Is.EqualTo(999.99m));
        Assert.That(result.CategoryId, Is.EqualTo(category.Id));
    }

    /// <summary>
    /// Verifies that updating a product modifies its data.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task UpdateModifiesProduct()
    {
        var category = await CreateTestCategory();
        var product = new Product { Name = "Old", CategoryId = category.Id, Price = 10m, Amount = 1 };
        await productRepository.Create(product);

        product.Name = "Updated";
        product.Price = 20m;
        await productRepository.Update(product);

        var result = await productRepository.GetById((int)product.Id);
        Assert.That(result.Name, Is.EqualTo("Updated"));
        Assert.That(result.Price, Is.EqualTo(20m));
    }

    /// <summary>
    /// Verifies that deleting a product removes it from the database.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task DeleteRemovesProduct()
    {
        var category = await CreateTestCategory();
        var product = new Product { Name = "ToDelete", CategoryId = category.Id, Price = 5m, Amount = 1 };
        await productRepository.Create(product);

        await productRepository.Delete((int)product.Id);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await productRepository.GetById((int)product.Id));
    }

    /// <summary>
    /// Verifies that deleting a category cascade-deletes its related products.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task DeleteCategoryCascadeDeletesRelatedProducts()
    {
        var category = await CreateTestCategory();
        var product1 = new Product { Name = "Product1", CategoryId = category.Id, Price = 10m, Amount = 1 };
        var product2 = new Product { Name = "Product2", CategoryId = category.Id, Price = 20m, Amount = 2 };
        await productRepository.Create(product1);
        await productRepository.Create(product2);

        await categoryRepository.Delete((int)category.Id);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await productRepository.GetById((int)product1.Id));
        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await productRepository.GetById((int)product2.Id));
    }
}