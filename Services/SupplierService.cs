using InventoryHub.Models;
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

        public SupplierService()
        {
            supplier = new List<Supplier>();
        }

        // Add
        public Supplier AddSupplier(string name, string contactInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Supplier name cannot be empty.");

            var newSupplier = new Supplier
            {
                Name = name,
                ContactInfo = contactInfo
            };

            supplier.Add(newSupplier);
            return newSupplier;
        }

        // Update 
        public Supplier UpdateSupplier(Supplier updatedSupplier)
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

            return existingSupplier;
        }

        // Delete
        public bool DeleteSupplier(Guid supplierId)
        {
            var _supplier = supplier.FirstOrDefault(s => s.Id == supplierId);
            if (supplier == null)
                throw new KeyNotFoundException("Supplier not found.");

            return supplier.Remove(_supplier);
        }

        // Get All Suppliers
        public List<Supplier> GetAllSuppliers()
        {
            return supplier;
        }
    }
}
