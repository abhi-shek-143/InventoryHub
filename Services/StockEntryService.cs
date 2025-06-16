using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public class StockEntryService : IStockEntryService
    {
        private readonly List<StockEntry> stockEntry;

        public StockEntryService()
        {
            stockEntry = new List<StockEntry>();
        }

        // Add
        public StockEntry AddStockEntry(Product product, int quantity, string entryType)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (entryType != "IN" && entryType != "OUT") //IN - adding stock, OUT - selling stock
                throw new ArgumentException("Entry type must be 'IN' or 'OUT'.");

            if (entryType == "OUT" && product.Quantity < quantity)
                throw new InvalidOperationException("Not enough stock available for OUT entry.");

            var newEntry = new StockEntry(product, quantity, DateTime.UtcNow, entryType);

            if (entryType == "IN")
                product.Quantity += quantity;
            else if (entryType == "OUT")
                product.Quantity -= quantity;

            stockEntry.Add(newEntry);
            return newEntry;
        }

        // Get 
        public List<StockEntry> GetAllStockEntries()
        {
            return stockEntry.OrderByDescending(e => e.EntryDate).ToList();
        }

        public List<StockEntry> GetStockEntriesByProduct(Guid productId)
        {
            return stockEntry
                .Where(e => e.Product.Id == productId)
                .OrderByDescending(e => e.EntryDate)
                .ToList();
        }
    }
}
