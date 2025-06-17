using InventoryHub.Models;
using InventoryHub.Persistence; // ✅ Added
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly List<Supplier> supplier;
        private const string FilePath = "suppliers.json"; // ✅ Added

        public SupplierService()
        {
            supplier = FileStorageHelper.LoadFromFileAsync<Supplier>(FilePath).Result; // ✅ Load
        }

        public async Task<Supplier> AddSupplier(string name, string contactInfo) // ✅ Changed to async
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier name cannot be empty.");

            var newSupplier = new Supplier
            {
                Name = name,
                ContactInfo = contactInfo
            };

            supplier.Add(newSupplier);
            await FileStorageHelper.SaveToFileAsync(FilePath, supplier); // ✅ Save
            return newSupplier;
        }

        public async Task<Supplier> UpdateSupplier(Supplier updatedSupplier) // ✅ Changed to async
        {
            if (updatedSupplier == null)
                throw new ArgumentNullException(nameof(updatedSupplier));

            var existingSupplier = supplier.FirstOrDefault(s => s.Id == updatedSupplier.Id);
            if (existingSupplier == null)
                throw new KeyNotFoundException("Supplier not found.");

            if (string.IsNullOrWhiteSpace(updatedSupplier.Name))
                throw new ArgumentException("Supplier name is required.");

            existingSupplier.Name = updatedSupplier.Name;
            existingSupplier.ContactInfo = updatedSupplier.ContactInfo;

            await FileStorageHelper.SaveToFileAsync(FilePath, supplier); // ✅ Save
            return existingSupplier;
        }

        public async Task<bool> DeleteSupplier(Guid supplierId) // ✅ Changed to async
        {
            var _supplier = supplier.FirstOrDefault(s => s.Id == supplierId);
            if (_supplier == null)
                throw new KeyNotFoundException("Supplier not found.");

            bool result = supplier.Remove(_supplier);
            await FileStorageHelper.SaveToFileAsync(FilePath, supplier); // ✅ Save
            return result;
        }

        public async Task<List<Supplier>> GetAllSuppliers() // ✅ Changed to async
        {
            return await Task.FromResult(supplier);
        }
    }
}
