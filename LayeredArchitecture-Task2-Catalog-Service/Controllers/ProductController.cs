using CatalogService.Business.Interfaces;
using CatalogService.Dtos;
using CatalogService.Dtos.Product;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers;

/// <summary>
/// Manages catalog products.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The product identifier.</param>
    /// <returns>The product with HATEOAS links.</returns>
    [HttpGet("{id}", Name = nameof(GetProductById))]
    [Authorize(Roles = "Manager,Customer")]
    [ProducesResponseType(typeof(LinkedResourceDto<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(int id)
    {
        var result = await productService.GetById(id);
        if (result == null)
            return NotFound();

        var links = new List<LinkDto>();
        AddLink(links, Url.Link(nameof(GetProductById), new { id }), "self", "GET");
        AddLink(links, Url.Link(nameof(UpdateProduct), null), "update_product", "PUT");
        AddLink(links, Url.Link(nameof(DeleteProduct), new { id }), "delete_product", "DELETE");
        AddLink(links, Url.Link(nameof(GetProducts), null), "all_products", "GET");

        var response = new LinkedResourceDto<ProductDto>
        {
            Data = result,
            Links = links
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
    [Authorize(Roles = "Manager,Customer")]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1)
            return BadRequest("Page must be greater than or equal to 1.");
        if (pageSize < 1 || pageSize > 50)
            return BadRequest("PageSize must be between 1 and 50.");

        var result = await productService.GetList(categoryId, page, pageSize);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productDto">The product data.</param>
    /// <returns>The created product.</returns>
    [HttpPost(Name = nameof(CreateProduct))]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(CreateProductDto productDto)
    {
        var id = await productService.Create(productDto);
        return CreatedAtAction(nameof(GetProductById), new { id }, productDto);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="productDto">The updated product data.</param>
    /// <returns>No content if successful.</returns>
    [HttpPut(Name = nameof(UpdateProduct))]
    [Authorize(Roles = "Manager")]
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
    /// <returns>No content if successful.</returns>
    [HttpDelete("{id}", Name = nameof(DeleteProduct))]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await productService.Delete(id);
        return NoContent();
    }

    private static void AddLink(List<LinkDto> links, string? href, string rel, string method)
    {
        if (href is not null)
            links.Add(new LinkDto { Href = href, Rel = rel, Method = method });
    }
}
