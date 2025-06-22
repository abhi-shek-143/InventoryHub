using NUnit.Framework;
using InventoryHub.Models;
using InventoryHub.Services;
using System;
using System.Threading.Tasks;

[TestFixture]
public class SupplierServiceTests
{
    private SupplierService _supplierService;

    [SetUp]
    public void Setup()
    {
        _supplierService = new SupplierService();
    }

    [Test]
    public async Task AddSupplier_ValidInput_ReturnsSupplier()
    {
        var result = await _supplierService.AddSupplier("Test Supplier", "test@example.com");

        Assert.IsNotNull(result);
        Assert.AreEqual("Test Supplier", result.Name);
    }

    [Test]
    public void AddSupplier_EmptyName_ThrowsException()
    {
        Assert.ThrowsAsync<ArgumentException>(() =>
            _supplierService.AddSupplier("", "test@example.com"));
    }

    [Test]
    public async Task UpdateSupplier_ValidUpdate_ModifiesData()
    {
        var supplier = await _supplierService.AddSupplier("Old Name", "contact");
        supplier.Name = "Updated Name";

        var updated = await _supplierService.UpdateSupplier(supplier);
        Assert.AreEqual("Updated Name", updated.Name);
    }

    [Test]
    public void UpdateSupplier_InvalidId_ThrowsKeyNotFound()
    {
        var fakeSupplier = new Supplier { Id = Guid.NewGuid(), Name = "Ghost" };

        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _supplierService.UpdateSupplier(fakeSupplier));
    }

    [Test]
    public async Task DeleteSupplier_ValidId_RemovesSupplier()
    {
        var supplier = await _supplierService.AddSupplier("ToDelete", "contact");
        bool deleted = await _supplierService.DeleteSupplier(supplier.Id);

        Assert.IsTrue(deleted);
    }

    [Test]
    public void DeleteSupplier_InvalidId_ThrowsKeyNotFound()
    {
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _supplierService.DeleteSupplier(Guid.NewGuid()));
    }
}
