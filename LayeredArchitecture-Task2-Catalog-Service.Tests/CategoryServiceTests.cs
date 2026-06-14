using CatalogService.Business.Implementation;
using CatalogService.Business.Interfaces;
using CatalogService.Dtos.Category;
using CatalogService.Repository.Models;

using Moq;

namespace CatalogService.Tests;

/// <summary>
/// Unit tests for the category service.
/// </summary>
[TestFixture]
public class CategoryServiceTests
{
    private Mock<ICategoryRepository> categoryRepositoryMock;
    private ICategoryService categoryService;

    /// <summary>
    /// Initializes test dependencies before each test.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        categoryRepositoryMock = new Mock<ICategoryRepository>();
        categoryService = new CategoryService(categoryRepositoryMock.Object);
    }

    /// <summary>
    /// Verifies that GetById returns the correct category DTO.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetByIdReturnsCorrectCategoryDto()
    {
        var category = new Category { Id = 1, Name = "Electronics", ImageURL = "img.png", ParentCategoryId = null };
        categoryRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(category);

        var result = await categoryService.GetById(1);

        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Electronics"));
        Assert.That(result.ImageURL, Is.EqualTo("img.png"));
        Assert.That(result.ParentCategoryId, Is.Null);
    }

    /// <summary>
    /// Verifies that GetList returns all categories.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task GetListReturnsAllCategories()
    {
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Electronics" },
            new() { Id = 2, Name = "Books" }
        };
        categoryRepositoryMock.Setup(r => r.GetList()).ReturnsAsync(categories);

        var result = await categoryService.GetList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Electronics"));
        Assert.That(result[1].Name, Is.EqualTo("Books"));
    }

    /// <summary>
    /// Verifies that Create calls the repository Create method.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task CreateCallsRepositoryCreate()
    {
        var dto = new CategoryDto { Name = "Toys", ImageURL = "toys.png", ParentCategoryId = null };

        await categoryService.Create(dto);

        categoryRepositoryMock.Verify(r => r.Create(It.Is<Category>(c =>
            c.Name == "Toys" && c.ImageURL == "toys.png")), Times.Once);
    }

    /// <summary>
    /// Verifies that Update calls the repository Update method.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task UpdateCallsRepositoryUpdate()
    {
        var dto = new CategoryDto { Id = 1, Name = "Updated", ImageURL = "updated.png", ParentCategoryId = 2 };

        await categoryService.Update(dto);

        categoryRepositoryMock.Verify(r => r.Update(It.Is<Category>(c =>
            c.Id == 1 && c.Name == "Updated" && c.ParentCategoryId == 2)), Times.Once);
    }

    /// <summary>
    /// Verifies that Delete calls the repository Delete method.
    /// </summary>
    /// <returns>A task representing the asynchronous test operation.</returns>
    [Test]
    public async Task DeleteCallsRepositoryDelete()
    {
        await categoryService.Delete(1);

        categoryRepositoryMock.Verify(r => r.Delete(1), Times.Once);
    }
}

