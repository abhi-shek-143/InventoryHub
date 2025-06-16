using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();       // Unique identifier
        public string Name { get; set; } = string.Empty;     // Product name
        public string Description { get; set; }
        public decimal Price { get; set; }                   // Price per unit
        public int Quantity { get; set; }                    // Current stock level
        public Guid SupplierId { get; set; }                 // Link to Supplier
    }
}
