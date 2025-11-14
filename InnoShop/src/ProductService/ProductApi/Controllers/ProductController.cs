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

    [HttpGet("get-all")]
    public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
    {
        try
        {
            return await service.GetAllProducts();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}