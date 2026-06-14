namespace CatalogService.Repository.Models;

public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageURL { get; set; }
    public long? ParentCategoryId { get; set; }
    public ICollection<Product> Products { get; set; } = [];
}
