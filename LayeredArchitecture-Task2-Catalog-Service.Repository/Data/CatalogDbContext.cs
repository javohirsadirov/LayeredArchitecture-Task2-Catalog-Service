using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using LayeredArchitecture_Task2_Catalog_Service.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace LayeredArchitecture_Task2_Catalog_Service.Repository.Data;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}
