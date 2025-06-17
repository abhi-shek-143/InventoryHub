using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public interface IStockEntryService
    {
        Task<StockEntry> AddStockEntry(Product product, int quantity, string entryType);
        Task<List<StockEntry>> GetAllStockEntries();
        Task<List<StockEntry>> GetStockEntriesByProduct(Guid productId);
    }
}
