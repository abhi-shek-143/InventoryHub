using NUnit.Framework;
using InventoryHub.Models;
using InventoryHub.Services;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

[TestFixture]
public class ProductServiceTests
{
    private ProductService _productService;
    private Supplier _mockSupplier;

    [SetUp]
    public void Setup()
    {
        _productService = new ProductService();
        _productService.ClearProductsForTest(); // Reset data for each test

        _mockSupplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = "Test Supplier",
            ContactInfo = "test@supplier.com"
        };
    }

    [Test]
    public async Task AddProduct_ValidData_ReturnsProduct()
    {
        var product = await _productService.AddProduct(
            "Test Product", "A test item", 20.5m, 10, _mockSupplier.Id);

        Assert.IsNotNull(product);
        Assert.AreEqual("Test Product", product.Name);
    }

    [Test]
    public void AddProduct_EmptyName_ThrowsException()
    {
        Assert.ThrowsAsync<ArgumentException>(() =>
            _productService.AddProduct("", "desc", 5m, 2, _mockSupplier.Id));
    }

    [Test]
    public void AddProduct_NegativePrice_ThrowsException()
    {
        Assert.ThrowsAsync<ArgumentException>(() =>
            _productService.AddProduct("Item", "desc", -1m, 5, _mockSupplier.Id));
    }

    [Test]
    public void AddProduct_NegativeQuantity_ThrowsException()
    {
        Assert.ThrowsAsync<ArgumentException>(() =>
            _productService.AddProduct("Item", "desc", 5m, -3, _mockSupplier.Id));
    }

    [Test]
    public async Task UpdateProduct_ChangesData()
    {
        var product = await _productService.AddProduct("Old", "desc", 1m, 1, _mockSupplier.Id);
        product.Name = "New Name";
        product.Price = 99.99m;

        var updated = await _productService.UpdateProduct(product);
        Assert.AreEqual("New Name", updated.Name);
        Assert.AreEqual(99.99m, updated.Price);
    }

    [Test]
    public void UpdateProduct_NotFound_ThrowsException()
    {
        var fake = new Product { Id = Guid.NewGuid(), Name = "Fake" };

        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _productService.UpdateProduct(fake));
    }

    [Test]
    public async Task DeleteProduct_RemovesItem()
    {
        var product = await _productService.AddProduct("ToDelete", "desc", 5m, 1, _mockSupplier.Id);
        var deleted = await _productService.DeleteProduct(product.Id);

        Assert.IsTrue(deleted);
    }

    [Test]
    public async Task DeleteProduct_InvalidId_ReturnsFalse()
    {
        var deleted = await _productService.DeleteProduct(Guid.NewGuid());
        Assert.IsFalse(deleted);
    }

    [Test]
    public async Task GetLowStockProducts_ReturnsCorrectItems()
    {
        await _productService.AddProduct("P1", "desc", 10m, 2, _mockSupplier.Id);
        await _productService.AddProduct("P2", "desc", 10m, 8, _mockSupplier.Id);

        var lowStock = await _productService.GetLowStockProducts(5);

        Assert.AreEqual(1, lowStock.Count);
        Assert.AreEqual("P1", lowStock[0].Name);
    }
}
