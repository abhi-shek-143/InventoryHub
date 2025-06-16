using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products;

        public ProductService()
        {
            _products = new List<Product>();
        }


        public Product AddProduct(string name, string description, decimal price, int quantity, Guid supplierId)
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
            return newProduct;
        }

        public bool DeleteProduct(Guid productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            return _products.Remove(product);
        }


        public Product UpdateProduct(Product updatedProduct)
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

            // Update product
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Quantity = updatedProduct.Quantity;
            existingProduct.SupplierId = updatedProduct.SupplierId;

            return existingProduct;
        }

        // Shared
        public List<Product> GetAllProducts()
        {
            return _products;
        }
    }
}
