using System;

namespace InventoryHub.Models
{
    public class StockEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid(); //Global Unique ID
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryType { get; set; }  // "IN" or "OUT"

        public StockEntry() { }

        public StockEntry(Product product, int quantity, DateTime entryDate, string entryType)
        {
            Product = product;
            Quantity = quantity;
            EntryDate = entryDate;
            EntryType = entryType;
        }
    }
}

