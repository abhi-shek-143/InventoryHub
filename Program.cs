using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryHub.Models;
using InventoryHub.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // Initialize services
        var productService = new ProductService();
        var supplierService = new SupplierService();
        var stockEntryService = new StockEntryService();

        Console.WriteLine("📦 InventoryHub Console App");

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Add Supplier");
            Console.WriteLine("2. Add Product");
            Console.WriteLine("3. Add Stock Entry");
            Console.WriteLine("4. Show All Products");
            Console.WriteLine("5. Show All Suppliers");
            Console.WriteLine("6. Show All Stock Entries");
            Console.WriteLine("7. Update Product");
            Console.WriteLine("8. Delete Product");
            Console.WriteLine("0. Exit");
            Console.Write("Enter option: ");

            var input = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (input)
                {
                    case "1":
                        Console.Write("Supplier Name: ");
                        var supplierName = Console.ReadLine();

                        Console.Write("Contact Info: ");
                        var contact = Console.ReadLine();

                        var newSupplier = await supplierService.AddSupplier(supplierName, contact);
                        Console.WriteLine($"Supplier added with ID: {newSupplier.Id}");
                        break;

                    case "2":
                        Console.Write("Product Name: ");
                        var productName = Console.ReadLine();

                        Console.Write("Description: ");
                        var description = Console.ReadLine();

                        Console.Write("Price: ");
                        decimal price = decimal.Parse(Console.ReadLine());

                        Console.Write("Quantity: ");
                        int quantity = int.Parse(Console.ReadLine());

                        Console.Write("Supplier ID: ");
                        Guid supplierId = Guid.Parse(Console.ReadLine());

                        var newProduct = await productService.AddProduct(productName, description, price, quantity, supplierId);
                        Console.WriteLine($"Product added with ID: {newProduct.Id}");
                        break;

                    case "3":
                        Console.Write("Product ID: ");
                        Guid productId = Guid.Parse(Console.ReadLine());

                        var product = (await productService.GetAllProducts()).FirstOrDefault(p => p.Id == productId);
                        if (product == null)
                        {
                            Console.WriteLine("Product not found.");
                            break;
                        }

                        Console.Write("Quantity: ");
                        int stockQty = int.Parse(Console.ReadLine());

                        Console.Write("Entry Type (IN/OUT): ");
                        string entryType = Console.ReadLine();

                        var entry = await stockEntryService.AddStockEntry(product, stockQty, entryType);
                        Console.WriteLine($"✅ Stock entry added on {entry.EntryDate}");
                        break;

                    case "4":
                        var allProducts = await productService.GetAllProducts();
                        foreach (var p in allProducts)
                            Console.WriteLine($"{p.Id} - {p.Name} ({p.Quantity} in stock)");
                        break;

                    case "5":
                        var allSuppliers = await supplierService.GetAllSuppliers();
                        foreach (var s in allSuppliers)
                            Console.WriteLine($"{s.Id} - {s.Name} ({s.ContactInfo})");
                        break;

                    case "6":
                        var allEntries = await stockEntryService.GetAllStockEntries();
                        foreach (var e in allEntries)
                            Console.WriteLine($"{e.EntryDate}: {e.Product.Name} - {e.EntryType} {e.Quantity}");
                        break;

                    case "7":
                        var productsToUpdate = await productService.GetAllProducts();
                        if (productsToUpdate.Count == 0)
                        {
                            Console.WriteLine("No products available to update.");
                            break;
                        }

                        foreach (var p in productsToUpdate)
                            Console.WriteLine($"{p.Id} - {p.Name}");

                        Console.Write("Enter Product ID to update: ");
                        Guid updateId = Guid.Parse(Console.ReadLine());

                        var productToUpdate = productsToUpdate.FirstOrDefault(p => p.Id == updateId);
                        if (productToUpdate == null)
                        {
                            Console.WriteLine("Product not found.");
                            break;
                        }

                        Console.Write("New Name (leave blank to keep current): ");
                        var newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName))
                            productToUpdate.Name = newName;

                        Console.Write("New Description (leave blank to keep current): ");
                        var newDesc = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newDesc))
                            productToUpdate.Description = newDesc;

                        Console.Write("New Price (leave blank to keep current): ");
                        var newPriceStr = Console.ReadLine();
                        if (decimal.TryParse(newPriceStr, out decimal newPrice))
                            productToUpdate.Price = newPrice;

                        Console.Write("New Quantity (leave blank to keep current): ");
                        var newQtyStr = Console.ReadLine();
                        if (int.TryParse(newQtyStr, out int newQty))
                            productToUpdate.Quantity = newQty;

                        await productService.UpdateProduct(productToUpdate);
                        Console.WriteLine("Product updated.");
                        break;

                    case "8":
                        var productsToDelete = await productService.GetAllProducts();
                        if (productsToDelete.Count == 0)
                        {
                            Console.WriteLine("No products available to delete.");
                            break;
                        }

                        foreach (var p in productsToDelete)
                            Console.WriteLine($"{p.Id} - {p.Name}");

                        Console.Write("Enter Product ID to delete: ");
                        Guid deleteId = Guid.Parse(Console.ReadLine());

                        bool deleted = await productService.DeleteProduct(deleteId);
                        Console.WriteLine(deleted ? "Product deleted." : "Product not found.");
                        break;

                    case "0":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
