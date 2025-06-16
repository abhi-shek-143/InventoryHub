using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public interface ISupplierService
    {
        Supplier AddSupplier(string name, string contactInfo);
        Supplier UpdateSupplier(Supplier updatedSupplier);
        bool DeleteSupplier(Guid supplierId);
        List<Supplier> GetAllSuppliers();
    }
}
