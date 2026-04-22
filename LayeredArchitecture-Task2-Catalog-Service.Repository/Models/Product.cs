using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LayeredArchitecture_Task2_Catalog_Service.Repository.Models;

public class Product
{
    public long Id { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageURL { get; set; }
    public required long CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
    [Precision(18, 2)]
    public required decimal Price { get; set; }
    public required int Amount { get; set; }
}