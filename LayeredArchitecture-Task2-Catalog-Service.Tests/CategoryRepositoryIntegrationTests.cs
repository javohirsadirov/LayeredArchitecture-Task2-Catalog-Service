using CatalogService.Business.Implementation;
using CatalogService.Repository.Data;
using CatalogService.Repository.Models;

using Microsoft.EntityFrameworkCore;

namespace CatalogService.Tests;

/// <summary>
/// Integration tests for the category repository.
/// </summary>
[TestFixture]
public class CategoryRepositoryIntegrationTests
{
    private CatalogDbContext context;
    private CategoryRepository repository;

    /// <summary>
    /// Initializes the database context and repository before each test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer("Server=localhost;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        context = new CatalogDbContext(options);
        context.Database.EnsureCreated();
        repository = new CategoryRepository(context);
    }

    /// <summary>
    /// Disposes the database context after each test.
    /// </summary>
    [TearDown]
    public void TearDown()
    {
        context.Dispose();
    }

    /// <summary>
    /// Verifies that a created category can be retrieved by its identifier.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task CreateAndGetByIdReturnsCreatedCategory()
    {
        var category = new Category { Name = "Electronics", ImageURL = "electronics.png" };

        await repository.Create(category);
        var result = await repository.GetById((int)category.Id);

        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo("Electronics"));
            Assert.That(result.ImageURL, Is.EqualTo("electronics.png"));
        });
    }

    /// <summary>
    /// Verifies that updating a category modifies its data.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task UpdateModifiesCategory()
    {
        var category = new Category { Name = "Old Name" };
        await repository.Create(category);

        category.Name = "New Name";
        await repository.Update(category);

        var result = await repository.GetById((int)category.Id);
        Assert.That(result.Name, Is.EqualTo("New Name"));
    }

    /// <summary>
    /// Verifies that deleting a category removes it from the database.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task DeleteRemovesCategory()
    {
        var category = new Category { Name = "ToDelete" };
        await repository.Create(category);

        await repository.Delete((int)category.Id);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await repository.GetById((int)category.Id));
    }
}