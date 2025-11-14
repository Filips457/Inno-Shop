using Microsoft.EntityFrameworkCore;
using ProductApplication.Interfaces;
using ProductDomain.Model;
using ProductInfrastructure.DataSources;
using ProductInfrastructure.Entities;

namespace ProductInfrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductContextMySql productContext;

    public ProductRepository(ProductContextMySql productContextMySql)
    {
        productContext = productContextMySql;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var products = await productContext.Products.ToListAsync();
        return products.Select(p => ConvertProduct(p)).ToList();
    }

    public async Task<Product> GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteProduct(int id)
    {
        throw new NotImplementedException();
    }

    private Product ConvertProduct(ProductEntity productEntity)
    {
        return new Product
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            Description = productEntity.Description,
            Price = productEntity.Price,
            CreationTime = productEntity.CreationTime,
            UserId = productEntity.UserId,
        };
    }

    private ProductEntity ConvertProduct(Product product)
    {
        return new ProductEntity
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