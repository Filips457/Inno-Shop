using ProductApplication.DTOs;
using ProductApplication.Exceptions;
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

    public async Task<List<ProductDTO>> GetActiveProducts()
    {
        var products = await repository.GetAllProducts();
        return products.Where(p => p.IsActive == true).Select(p => ConvertProduct(p)).ToList();
    }

    public async Task<ProductDTO> GetProductById(int id)
    {
        var product = await repository.GetProductById(id);

        if (product == null)
            throw new NotFoundException($"Product with id {id} was not found.");

        return ConvertProduct(product);
    }

    public async Task<ProductDTO> CreateProduct(ProductRequestDTO productDTO)
    {
        var product = ConvertProduct(productDTO);
        product.Id = 0;
        product = await repository.CreateProduct(product);

        return ConvertProduct(product);
    }

    public async Task UpdateProduct(int id, ProductRequestDTO productDTO)
    {
        var product = await repository.GetProductById(id);
        if (product == null)
            throw new NotFoundException($"Product with id {id} was not found.");

        product.Name = productDTO.Name;
        product.Description = productDTO.Description;
        product.Price = productDTO.Price;
        product.UserId = productDTO.UserId;

        await repository.UpdateProduct(product);
    }

    public async Task SetProductsActive(int userId, bool isActive)
    {
        await repository.SetProductsActive(userId, isActive);
    }

    public async Task DeleteProduct(int id)
    {
        var product = await repository.GetProductById(id);
        if (product == null)
            throw new NotFoundException($"Product with id {id} was not found.");

        await repository.DeleteProduct(id);
    }


    private Product ConvertProduct(ProductRequestDTO productDTO)
    {
        return new Product
        {
            Name = productDTO.Name,
            Description = productDTO.Description,
            Price = productDTO.Price,
            CreationTime = DateTime.UtcNow,
            IsActive = productDTO.IsActive,
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
            IsActive = product.IsActive,
            UserId = product.UserId,
        };
    }
}