using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProducts(CancellationToken cancellationToken = default);
        Task<Product?> GetProductById(int id, CancellationToken cancellationToken = default);
        Task<Product> CreateProduct(Product product, CancellationToken cancellationToken = default);
        Task<Product?> UpdateProduct(int id, Product product, CancellationToken cancellationToken = default);
        Task<bool> DeleteProduct(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Product>> SearchByCategory(string category, CancellationToken cancellationToken = default);
    }
}