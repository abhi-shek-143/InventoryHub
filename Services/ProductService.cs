using InventoryHub.Models;
using InventoryHub.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<Product> UpdateProduct(Product updatedProduct)
        {
            if (updatedProduct == null)
                throw new ArgumentNullException(nameof(updatedProduct));

            var existingProduct = _products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct == null)
                throw new KeyNotFoundException("Product not found.");

            if (string.IsNullOrWhiteSpace(updatedProduct.Name))
                throw new ArgumentException("Product name is required.");
            if (updatedProduct.Price < 0 || updatedProduct.Quantity < 0)
                throw new ArgumentException("Price and quantity must be non-negative.");

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Quantity = updatedProduct.Quantity;
            existingProduct.SupplierId = updatedProduct.SupplierId;

            await FileStorageHelper.SaveToFileAsync(FilePath, _products);
            return existingProduct;
        }

        public async Task<bool> DeleteProduct(Guid productId) //async method
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            bool result = _products.Remove(product);
            await FileStorageHelper.SaveToFileAsync(FilePath, _products);
            return result;
        }

        public Task<List<Product>> GetAllProducts()
        {
            return Task.FromResult(_products);
        }
    }
}
