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

    public async Task<Product?> GetProductById(int id)
    {
        var product = await productContext.Products.FindAsync(id);
        return product != null ? ConvertProduct(product) : null;
    }

    public async Task<Product> CreateProduct(Product product)
    {
        ProductEntity productToInsert = ConvertProduct(product);

        await productContext.AddAsync(productToInsert);
        await productContext.SaveChangesAsync();

        return ConvertProduct(productToInsert);
    }

    public async Task UpdateProduct(Product product)
    {
        var productToUpdate = await productContext.Products.FindAsync(product.Id);
        if (productToUpdate == null)
            return;

        productToUpdate.Name = product.Name;
        productToUpdate.Description = product.Description;
        productToUpdate.Price = product.Price;
        productToUpdate.CreationTime = product.CreationTime;

        productToUpdate.UserId = product.UserId;

        await productContext.SaveChangesAsync();
    }

    public async Task SetProductsActive(int userId, bool isActive)
    {
        var products = productContext.Products.Where(p => p.UserId == userId);

        foreach (var product in products)
            product.IsActive = isActive;

        await productContext.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        var productToDelete = await productContext.Products.FindAsync(id);
        if (productToDelete == null)
            return;

        productContext.Products.Remove(productToDelete);
        await productContext.SaveChangesAsync();
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