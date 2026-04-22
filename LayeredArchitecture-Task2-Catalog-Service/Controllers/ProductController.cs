using LayeredArchitecture_Task2_Catalog_Service.Business.Interfaces;
using LayeredArchitecture_Task2_Catalog_Service.Dtos.Product;
using Microsoft.AspNetCore.Mvc;

namespace LayeredArchitecture_Task2_Catalog_Service.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await productService.GetById(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await productService.GetList();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto productDto)
    {
        await productService.Create(productDto);
        return Ok();
    }

    [HttpPut()]
    public async Task<IActionResult> Update(ProductDto productDto)
    {
        await productService.Update(productDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await productService.Delete(id);
        return Ok();
    }
}