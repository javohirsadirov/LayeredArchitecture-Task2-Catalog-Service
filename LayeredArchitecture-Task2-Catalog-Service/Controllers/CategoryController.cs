using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using Microsoft.AspNetCore.Mvc;

namespace LayeredArchitecture_Task2_Catalog_Service.Controllers;

/// <summary>
/// Manages catalog categories.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <returns>The category with HATEOAS links.</returns>
    [HttpGet("{id}", Name = nameof(GetCategoryById))]
    [ProducesResponseType(typeof(LinkedResourceDto<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var result = await categoryService.GetById(id);
        if (result == null)
            return NotFound();

        var links = new List<LinkDto>();
        AddLink(links, Url.Link(nameof(GetCategoryById), new { id }), "self", "GET");
        AddLink(links, Url.Link(nameof(UpdateCategory), null), "update_category", "PUT");
        AddLink(links, Url.Link(nameof(DeleteCategory), new { id }), "delete_category", "DELETE");
        AddLink(links, Url.Link(nameof(GetCategories), null), "all_categories", "GET");

        var response = new LinkedResourceDto<CategoryDto>
        {
            Data = result,
            Links = links
        };

        return Ok(response);
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>A list of categories.</returns>
    [HttpGet(Name = nameof(GetCategories))]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
    {
        var result = await categoryService.GetList();
        return Ok(result);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="categoryDto">The category data.</param>
    /// <returns>The created category.</returns>
    [HttpPost(Name = nameof(CreateCategory))]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
    {
        await categoryService.Create(categoryDto);
        return CreatedAtAction(nameof(GetCategoryById), new { id = categoryDto.Id }, categoryDto);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    /// <param name="categoryDto">The updated category data.</param>
    [HttpPut(Name = nameof(UpdateCategory))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(CategoryDto categoryDto)
    {
        await categoryService.Update(categoryDto);
        return NoContent();
    }

    /// <summary>
    /// Deletes a category and its related products.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    [HttpDelete("{id}", Name = nameof(DeleteCategory))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await categoryService.Delete(id);
        return NoContent();
    }

    private static void AddLink(List<LinkDto> links, string? href, string rel, string method)
    {
        if (href is not null)
            links.Add(new LinkDto { Href = href, Rel = rel, Method = method });
    }
}