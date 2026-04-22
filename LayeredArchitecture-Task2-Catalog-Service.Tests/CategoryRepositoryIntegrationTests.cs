using LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace LayeredArchitecture_Task2_Catalog_Service.Tests;

[TestFixture]
public class CategoryRepositoryIntegrationTests
{
    private CatalogDbContext _context;
    private CategoryRepository _repository;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseSqlServer("Server=localhost;Database=CatalogDb;Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        _context = new CatalogDbContext(options);
        _context.Database.EnsureCreated();
        _repository = new CategoryRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task Create_And_GetById_ReturnsCreatedCategory()
    {
        var category = new Category { Name = "Electronics", ImageURL = "electronics.png" };

        await _repository.Create(category);
        var result = await _repository.GetById((int)category.Id);

        Assert.That(result.Name, Is.EqualTo("Electronics"));
        Assert.That(result.ImageURL, Is.EqualTo("electronics.png"));
    }


    [Test]
    public async Task Update_ModifiesCategory()
    {
        var category = new Category { Name = "Old Name" };
        await _repository.Create(category);

        category.Name = "New Name";
        await _repository.Update(category);

        var result = await _repository.GetById((int)category.Id);
        Assert.That(result.Name, Is.EqualTo("New Name"));
    }

    [Test]
    public async Task Delete_RemovesCategory()
    {
        var category = new Category { Name = "ToDelete" };
        await _repository.Create(category);

        await _repository.Delete((int)category.Id);

        Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _repository.GetById((int)category.Id));
    }
}
