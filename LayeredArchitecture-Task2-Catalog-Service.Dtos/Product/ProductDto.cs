using System.ComponentModel.DataAnnotations;

namespace LayeredArchitecture_Task2_Catalog_Service.Dtos.Product;

public class ProductDto
{
    public long Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageURL { get; set; }
    public required long CategoryId { get; set; }
    public required decimal Price { get; set; }
    public required int Amount { get; set; }
}