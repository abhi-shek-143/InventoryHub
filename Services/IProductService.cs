using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public interface IProductService
    {
        Task<Product> AddProduct(string name, string description, decimal price, int quantity, Guid supplierId);
        Task<Product> UpdateProduct(Product updatedProduct);
        Task<bool> DeleteProduct(Guid productId);
        Task<List<Product>> GetAllProducts();

        // 🔍 LINQ features
        Task<List<Product>> GetLowStockProducts(int threshold);
        Task<List<Product>> GetProductsBySupplier(Guid supplierId);
    }
}
