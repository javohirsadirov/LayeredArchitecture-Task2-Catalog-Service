using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;

namespace LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string? ImageURL { get; set; }
    public long? ParentCategoryId { get; set; }
    public ICollection<Product> Products { get; set; } = [];
}