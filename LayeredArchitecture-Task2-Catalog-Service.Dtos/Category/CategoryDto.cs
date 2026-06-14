namespace CatalogService.Dtos.Category;

public class CategoryDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? ImageURL { get; set; }
    public long? ParentCategoryId { get; set; }
}
