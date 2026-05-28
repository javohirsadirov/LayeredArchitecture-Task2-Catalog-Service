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
    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public required decimal Price { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Amount cannot be negative.")]
    public required int Amount { get; set; }
}