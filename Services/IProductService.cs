using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public interface IProductService
    {
        Product AddProduct(string name, string description, decimal price, int quantity, Guid supplierId);
        Product UpdateProduct(Product updatedProduct);
        bool DeleteProduct(Guid productId);
        List<Product> GetAllProducts();
    }
}
