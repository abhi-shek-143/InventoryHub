using InventoryHub.Models;
using InventoryHub.Services;
using System;
using System.Threading.Tasks;

namespace InventoryHub
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await MainAsync();
        }

        static async Task MainAsync()
        {
            Console.WriteLine("Welcome to InventoryHub!");

            var productService = new ProductService();
            var supplierService = new SupplierService();
            var stockEntryService = new StockEntryService();

            // Add a supplier
            var newSupplier = await supplierService.AddSupplier("ABC Ltd.", "abc@example.com");
            Console.WriteLine($"Supplier Added: {newSupplier.Name}");

            // Add a product
            var product = await productService.AddProduct("Laptop", "Dell i5", 50000, 10, newSupplier.Id);
            Console.WriteLine($"Product Added: {product.Name} - Qty: {product.Quantity}");

            // Add stock entry
            var entry = await stockEntryService.AddStockEntry(product, 5, "IN");
            Console.WriteLine($"Stock Entry Added: {entry.EntryType} {entry.Quantity} for {entry.Product.Name}");

            // View all products
            var allProducts = await productService.GetAllProducts();
            Console.WriteLine("\nAll Products:");
            foreach (var p in allProducts)
            {
                Console.WriteLine($"- {p.Name}, Qty: {p.Quantity}, Price: {p.Price}");
            }
        }
    }
}
