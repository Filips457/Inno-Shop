using ProductApplication.DTOs;
using ProductApplication.Interfaces;
using ProductDomain.Model;

namespace ProductApplication.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository repository;

    public ProductService(IProductRepository productRepository)
    {
        repository = productRepository;
    }

    public async Task<List<ProductDTO>> GetAllProducts()
    {
        var products = await repository.GetAllProducts();
        return products.Select(p => ConvertProduct(p)).ToList();
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ProductDTO> CreateProduct(ProductDTO productDTO)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateProduct(int id, ProductDTO productDTO)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    private Product ConvertProduct(ProductDTO productDTO)
    {
        return new Product
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            CreationTime = productDTO.CreationTime,
            UserId = productDTO.UserId,
        };
    }

    private ProductDTO ConvertProduct(Product product)
    {
        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CreationTime = product.CreationTime,
            UserId = product.UserId,
        };
    }
}