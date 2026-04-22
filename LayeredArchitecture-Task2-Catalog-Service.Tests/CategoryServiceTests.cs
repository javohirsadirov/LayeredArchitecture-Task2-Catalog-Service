using LayeredArchitecture_Task2_Catalog_Service.Business.Implementation;
using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using Moq;

namespace LayeredArchitecture_Task2_Catalog_Service.Tests;

[TestFixture]
public class CategoryServiceTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private ICategoryService _categoryService;

    [SetUp]
    public void SetUp()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object);
    }

    [Test]
    public async Task GetById_ReturnsCorrectCategoryDto()
    {
        var category = new Category { Id = 1, Name = "Electronics", ImageURL = "img.png", ParentCategoryId = null };
        _categoryRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(category);

        var result = await _categoryService.GetById(1);

        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Electronics"));
        Assert.That(result.ImageURL, Is.EqualTo("img.png"));
        Assert.That(result.ParentCategoryId, Is.Null);
    }

    [Test]
    public async Task GetList_ReturnsAllCategories()
    {
        var categories = new List<Category>
        {
            new() { Id = 1, Name = "Electronics" },
            new() { Id = 2, Name = "Books" }
        };
        _categoryRepositoryMock.Setup(r => r.GetList()).ReturnsAsync(categories);

        var result = await _categoryService.GetList();

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Electronics"));
        Assert.That(result[1].Name, Is.EqualTo("Books"));
    }

    [Test]
    public async Task Create_CallsRepositoryCreate()
    {
        var dto = new CategoryDto { Name = "Toys", ImageURL = "toys.png", ParentCategoryId = null };

        await _categoryService.Create(dto);

        _categoryRepositoryMock.Verify(r => r.Create(It.Is<Category>(c =>
            c.Name == "Toys" && c.ImageURL == "toys.png")), Times.Once);
    }

    [Test]
    public async Task Update_CallsRepositoryUpdate()
    {
        var dto = new CategoryDto { Id = 1, Name = "Updated", ImageURL = "updated.png", ParentCategoryId = 2 };

        await _categoryService.Update(dto);

        _categoryRepositoryMock.Verify(r => r.Update(It.Is<Category>(c =>
            c.Id == 1 && c.Name == "Updated" && c.ParentCategoryId == 2)), Times.Once);
    }

    [Test]
    public async Task Delete_CallsRepositoryDelete()
    {
        await _categoryService.Delete(1);

        _categoryRepositoryMock.Verify(r => r.Delete(1), Times.Once);
    }
}
