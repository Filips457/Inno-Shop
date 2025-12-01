using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.DTOs;
using ProductApplication.Interfaces;

namespace ProductApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService service;

    public ProductController(IProductService productService)
    {
        service = productService;
    }

    [HttpGet("All-products")]
    public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
    {
        return await service.GetAllProducts();
    }

    [HttpGet("Active-products")]
    public async Task<ActionResult<List<ProductDTO>>> GetActiveProducts()
    {
        return await service.GetActiveProducts();
    }

    [HttpGet("Product-by-id/{id}")]
    public async Task<ActionResult<ProductDTO>> GetProductById([FromRoute] int id)
    {
        return await service.GetProductById(id);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductRequestDTO productRequestDTO)
    {
        return await CreateProduct(productRequestDTO);
    }

    [Authorize]
    [HttpPut("update-product/{id}")]
    public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductRequestDTO productRequestDTO)
    {
        await service.UpdateProduct(id, productRequestDTO);
        return NoContent();
    }

    [Authorize]
    [HttpPut("set-active/{userId}/{isActive}")]
    public async Task<IActionResult> SetProductActive([FromRoute] int userId, [FromRoute] bool isActive)
    {
        await service.SetProductsActive(userId, isActive);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        await service.DeleteProduct(id);
        return Ok();
    }
}