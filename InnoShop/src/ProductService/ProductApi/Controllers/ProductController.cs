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

    [HttpGet("Product-by-id")]
    public async Task<ActionResult<ProductDTO>> GetProductById([FromQuery] int id)
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
    [HttpPut("update-product")]
    public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromBody] ProductRequestDTO productRequestDTO)
    {
        await service.UpdateProduct(id, productRequestDTO);
        return NoContent();
    }

    [HttpPost("set-active/{id}/{isActive}")]
    public async Task<IActionResult> SetProductActive([FromQuery] int userId, [FromQuery] bool isActive)
    {
        await service.SetProductsActive(userId, isActive);
        return NoContent();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteProduct([FromQuery] int id)
    {
        await service.DeleteProduct(id);
        return Ok();
    }
}