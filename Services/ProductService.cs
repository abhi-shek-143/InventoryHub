using InventoryHub.Models;
using InventoryHub.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products;
        private const string FilePath = "products.json";

        public ProductService()
        {
            _products = FileStorageHelper.LoadFromFileAsync<Product>(FilePath).Result;
        }

        public async Task<Product> AddProduct(string name, string description, decimal price, int quantity, Guid supplierId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");
            if (price < 0 || quantity < 0)
                throw new ArgumentException("Price and quantity must be non-negative.");

            var newProduct = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Quantity = quantity,
                SupplierId = supplierId
            };

            _products.Add(newProduct);
            await FileStorageHelper.SaveToFileAsync(FilePath, _products);
            return newProduct;
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return false;

            bool result = _products.Remove(product);
            await FileStorageHelper.SaveToFileAsync(FilePath, _products);
            return result;
        }

        public async Task<Product> UpdateProduct(Product updatedProduct)
        {
            var existing = _products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existing == null)
                throw new KeyNotFoundException("Product not found.");

            existing.Name = updatedProduct.Name;
            existing.Description = updatedProduct.Description;
            existing.Price = updatedProduct.Price;
            existing.Quantity = updatedProduct.Quantity;
            existing.SupplierId = updatedProduct.SupplierId;

            await FileStorageHelper.SaveToFileAsync(FilePath, _products);
            return existing;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await Task.FromResult(_products);
        }

        // 🔍 LINQ Feature 1: Low-stock products
        public async Task<List<Product>> GetLowStockProducts(int threshold)
        {
            return await Task.FromResult(
                _products.Where(p => p.Quantity <= threshold).ToList()
            );
        }

        // 🔍 LINQ Feature 2: Products by supplier
        public async Task<List<Product>> GetProductsBySupplier(Guid supplierId)
        {
            return await Task.FromResult(
                _products.Where(p => p.SupplierId == supplierId).ToList()
            );
        }

#if DEBUG
        public void ClearProductsForTest()
        {
            _products.Clear();
            FileStorageHelper.SaveToFileAsync(FilePath, _products).Wait();
        }
#endif

    }
}
