using InventoryHub.Models;
using InventoryHub.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryHub.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly List<Supplier> _suppliers;
        private const string FilePath = "suppliers.json";

        public SupplierService()
        {
            _suppliers = FileStorageHelper.LoadFromFileAsync<Supplier>(FilePath).Result;
        }

        public async Task<Supplier> AddSupplier(string name, string contactInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier name cannot be empty.");

            var newSupplier = new Supplier
            {
                Name = name,
                ContactInfo = contactInfo
            };

            _suppliers.Add(newSupplier);
            await FileStorageHelper.SaveToFileAsync(FilePath, _suppliers);
            return newSupplier;
        }

        public async Task<Supplier> UpdateSupplier(Supplier updatedSupplier)
        {
            if (updatedSupplier == null)
                throw new ArgumentNullException(nameof(updatedSupplier));

            var existing = _suppliers.FirstOrDefault(s => s.Id == updatedSupplier.Id);
            if (existing == null)
                throw new KeyNotFoundException("Supplier not found.");

            if (string.IsNullOrWhiteSpace(updatedSupplier.Name))
                throw new ArgumentException("Supplier name is required.");

            existing.Name = updatedSupplier.Name;
            existing.ContactInfo = updatedSupplier.ContactInfo;

            await FileStorageHelper.SaveToFileAsync(FilePath, _suppliers);
            return existing;
        }

        public async Task<bool> DeleteSupplier(Guid supplierId)
        {
            var supplier = _suppliers.FirstOrDefault(s => s.Id == supplierId);
            if (supplier == null)
                throw new KeyNotFoundException("Supplier not found.");

            bool result = _suppliers.Remove(supplier);
            await FileStorageHelper.SaveToFileAsync(FilePath, _suppliers);
            return result;
        }

        public Task<List<Supplier>> GetAllSuppliers()
        {
            return Task.FromResult(_suppliers);
        }
    }
}
