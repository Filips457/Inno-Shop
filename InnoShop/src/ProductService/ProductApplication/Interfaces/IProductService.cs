using ProductApplication.DTOs;

namespace ProductApplication.Interfaces;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllProducts();

    Task<List<ProductDTO>> GetActiveProducts();

    Task<ProductDTO> GetProductById(int id);

    Task<ProductDTO> CreateProduct(ProductRequestDTO productDTO);

    Task UpdateProduct(int id, ProductRequestDTO productDTO);

    Task SetProductsActive(int userId, bool isActive);

    Task DeleteProduct(int id);
}