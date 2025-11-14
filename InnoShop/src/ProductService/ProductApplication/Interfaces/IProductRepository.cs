using ProductApplication.DTOs;
using ProductDomain.Model;

namespace ProductApplication.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProducts();

    Task<Product> GetProductById(int id);

    Task<Product> CreateProduct(Product product);

    Task UpdateProduct(Product product);

    Task DeleteProduct(int id);
}