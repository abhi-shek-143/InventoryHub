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
        private const string FilePath = "stockentries.json";

        public StockEntryService()
        {
            _entries = FileStorageHelper.LoadFromFileAsync<StockEntry>(FilePath).Result;
        }

        public async Task<StockEntry> AddStockEntry(Product product, int quantity, string entryType)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (entryType.ToUpper() != "IN" && entryType.ToUpper() != "OUT")
                throw new ArgumentException("Entry type must be IN or OUT.");

            if (entryType.ToUpper() == "OUT" && quantity > product.Quantity)
                throw new InvalidOperationException("Insufficient stock.");

            var entry = new StockEntry
            {
                Product = product,
                Quantity = quantity,
                EntryType = entryType.ToUpper(),
                EntryDate = DateTime.Now
            };

            if (entry.EntryType == "IN")
                product.Quantity += quantity;
            else
                product.Quantity -= quantity;

            _entries.Add(entry);
            await FileStorageHelper.SaveToFileAsync(FilePath, _entries);
            return entry;
        }

        public async Task<List<StockEntry>> GetAllStockEntries()
        {
            return await Task.FromResult(_entries);
        }

        // 🔍 LINQ Feature 3: Filter entries by IN or OUT
        public async Task<List<StockEntry>> FilterStockEntries(string entryType)
        {
            return await Task.FromResult(
                _entries.Where(e => e.EntryType.Equals(entryType, StringComparison.OrdinalIgnoreCase)).ToList()
            );
        }
    }
}
