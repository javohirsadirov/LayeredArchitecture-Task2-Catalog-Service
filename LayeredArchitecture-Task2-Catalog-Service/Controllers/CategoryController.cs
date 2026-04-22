using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Category;
using Microsoft.AspNetCore.Mvc;

namespace LayeredArchitecture_Task2_Catalog_Service.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await categoryService.GetById(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await categoryService.GetList();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto categoryDto)
    {
        await categoryService.Create(categoryDto);
        return Ok();
    }

    [HttpPut()]
    public async Task<IActionResult> Update(CategoryDto categoryDto)
    {
        await categoryService.Update(categoryDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await categoryService.Delete(id);
        return Ok();
    }
}