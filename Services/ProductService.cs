using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data;
using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            AppDbContext context,
            ILogger<ProductService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Product>> GetAllProducts(
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetProductById(
            int id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FindAsync([id], cancellationToken);
        }

        public async Task<Product> CreateProduct(
            Product product,
            CancellationToken cancellationToken = default)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Product Created: {ProductId} ({ProductName})",
                product.Id,
                product.Name);

            return product;
        }

        public async Task<Product?> UpdateProduct(
            int id,
            Product product,
            CancellationToken cancellationToken = default)
        {
            var existing = await _context.Products
                .FindAsync([id], cancellationToken);

            if (existing is null)
                return null;

            existing.Name = product.Name;
            existing.Category = product.Category;
            existing.Price = product.Price;
            existing.Stock = product.Stock;
            existing.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Product Updated: {ProductId}",
                id);

            return existing;
        }

        public async Task<bool> DeleteProduct(
            int id,
            CancellationToken cancellationToken = default)
        {
            var product = await _context.Products
                .FindAsync([id], cancellationToken);

            if (product is null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Product Deleted: {ProductId}",
                id);

            return true;
        }

        public async Task<IReadOnlyList<Product>> SearchByCategory(
            string category,
            CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Category == category)
                .ToListAsync(cancellationToken);
        }
    }
}