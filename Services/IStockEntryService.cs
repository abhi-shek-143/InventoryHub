using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public interface IStockEntryService
    {
        StockEntry AddStockEntry(Product product, int quantity, string entryType);
        List<StockEntry> GetAllStockEntries();
        List<StockEntry> GetStockEntriesByProduct(Guid productId);
    }
}
