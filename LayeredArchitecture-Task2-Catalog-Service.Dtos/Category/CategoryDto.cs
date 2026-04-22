namespace LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;

public class CategoryDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? ImageURL { get; set; }
    public long? ParentCategoryId { get; set; }
}