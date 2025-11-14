using ProductApplication.DTOs;

namespace ProductApplication.Interfaces;

public interface IProductService
{
    Task<List<ProductDTO>> GetAllProducts();

    Task<ProductDTO> GetProductById(int id);

    Task<ProductDTO> CreateProduct(ProductDTO productDTO);

    Task UpdateProduct(int id, ProductDTO productDTO);

    Task DeleteProduct(int id);
}