using InventoryHub.Models;
using InventoryHub.Persistence; // ✅ Added
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
        private const string FilePath = "stockentries.json"; // ✅ Added

        public StockEntryService()
        {
            stockEntry = FileStorageHelper.LoadFromFileAsync<StockEntry>(FilePath).Result; // ✅ Load
        }

        public async Task<StockEntry> AddStockEntry(Product product, int quantity, string entryType) // ✅ Changed to async
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (entryType != "IN" && entryType != "OUT")
                throw new ArgumentException("Entry type must be 'IN' or 'OUT'.");

            if (entryType == "OUT" && product.Quantity < quantity)
                throw new InvalidOperationException("Not enough stock available for OUT entry.");

            var newEntry = new StockEntry(product, quantity, DateTime.UtcNow, entryType);

            if (entryType == "IN")
                product.Quantity += quantity;
            else if (entryType == "OUT")
                product.Quantity -= quantity;

            stockEntry.Add(newEntry);
            await FileStorageHelper.SaveToFileAsync(FilePath, stockEntry); // ✅ Save
            return newEntry;
        }

        public async Task<List<StockEntry>> GetAllStockEntries() // ✅ Changed to async
        {
            return await Task.FromResult(stockEntry.OrderByDescending(e => e.EntryDate).ToList());
        }

        public async Task<List<StockEntry>> GetStockEntriesByProduct(Guid productId) // ✅ Changed to async
        {
            return await Task.FromResult(stockEntry
                .Where(e => e.Product.Id == productId)
                .OrderByDescending(e => e.EntryDate)
                .ToList());
        }
    }
}
