using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Models
{
    public class Supplier
    {
        public Guid Id { get; set; } = Guid.NewGuid();       // Unique identifier
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; }
    }
}
