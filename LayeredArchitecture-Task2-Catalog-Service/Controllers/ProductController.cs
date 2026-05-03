using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Product;
using Microsoft.AspNetCore.Mvc;

namespace LayeredArchitecture_Task2_Catalog_Service.Controllers;

/// <summary>
/// Manages catalog products.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>The product with HATEOAS links.</returns>
    [HttpGet("{id}", Name = nameof(GetProductById))]
    [ProducesResponseType(typeof(LinkedResourceDto<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await productService.GetById(id);
        if (result == null)
            return NotFound();

        var response = new LinkedResourceDto<ProductDto>
        {
            Data = result,
            Links =
            [
                new LinkDto { Href = Url.Link(nameof(GetProductById), new { id })!, Rel = "self", Method = "GET" },
                new LinkDto { Href = Url.Link(nameof(UpdateProduct), null)!, Rel = "update_product", Method = "PUT" },
                new LinkDto { Href = Url.Link(nameof(DeleteProduct), new { id })!, Rel = "delete_product", Method = "DELETE" },
                new LinkDto { Href = Url.Link(nameof(GetProducts), null)!, Rel = "all_products", Method = "GET" }
            ]
        };

        return Ok(response);
    }

    /// <summary>
    /// Gets a paginated list of products, optionally filtered by category.
    /// </summary>
    /// <param name="categoryId">Optional category identifier to filter by.</param>
    /// <param name="page">Page number (default: 1).</param>
    /// <param name="pageSize">Number of items per page (default: 10).</param>
    /// <returns>A list of products.</returns>
    [HttpGet(Name = nameof(GetProducts))]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await productService.GetList(categoryId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productDto">The product data.</param>
    /// <returns>The created product.</returns>
    [HttpPost(Name = nameof(CreateProduct))]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(ProductDto productDto)
    {
        await productService.Create(productDto);
        return CreatedAtAction(nameof(GetProductById), new { id = productDto.Id }, productDto);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="productDto">The updated product data.</param>
    [HttpPut(Name = nameof(UpdateProduct))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(ProductDto productDto)
    {
        await productService.Update(productDto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    [HttpDelete("{id}", Name = nameof(DeleteProduct))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await productService.Delete(id);
        return NoContent();
    }
}