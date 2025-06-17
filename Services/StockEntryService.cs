using InventoryHub.Models;
using InventoryHub.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public class StockEntryService : IStockEntryService
    {
        private readonly List<StockEntry> _entries;
        private const string FilePath = "stockEntries.json";

        public StockEntryService()
        {
            _entries = FileStorageHelper.LoadFromFileAsync<StockEntry>(FilePath).Result;
        }

        public async Task<StockEntry> AddStockEntry(Product product, int quantity, string entryType) //async method
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            if (entryType != "IN" && entryType != "OUT")
                throw new ArgumentException("Entry type must be 'IN' or 'OUT'.");

            if (entryType == "OUT" && product.Quantity < quantity)
                throw new InvalidOperationException("Not enough stock available for OUT entry.");

            var newEntry = new StockEntry(product, quantity, DateTime.UtcNow, entryType);

            if (entryType == "IN")
                product.Quantity += quantity;
            else
                product.Quantity -= quantity;

            _entries.Add(newEntry);
            await FileStorageHelper.SaveToFileAsync(FilePath, _entries);
            return newEntry;
        }

        public Task<List<StockEntry>> GetAllStockEntries()
        {
            return Task.FromResult(_entries.OrderByDescending(e => e.EntryDate).ToList());
        }

        public Task<List<StockEntry>> GetStockEntriesByProduct(Guid productId)
        {
            var filtered = _entries
                .Where(e => e.Product.Id == productId)
                .OrderByDescending(e => e.EntryDate)
                .ToList();

            return Task.FromResult(filtered);
        }
    }
}
