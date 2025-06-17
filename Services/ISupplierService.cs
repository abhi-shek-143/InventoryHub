using InventoryHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public interface ISupplierService
    {
        Task<Supplier> AddSupplier(string name, string contactInfo);
        Task<Supplier> UpdateSupplier(Supplier updatedSupplier);
        Task<bool> DeleteSupplier(Guid supplierId);
        Task<List<Supplier>> GetAllSuppliers();
    }
}
