using NUnit.Framework;
using InventoryHub.Models;
using InventoryHub.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[TestFixture]
public class StockEntryServiceTests
{
    private StockEntryService _stockEntryService;
    private Product _product;

    [SetUp]
    public void Setup()
    {
        _stockEntryService = new StockEntryService();
        _product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Test Product",
            Quantity = 10,
            Price = 50
        };
    }

    [Test]
    public async Task AddStockEntry_IN_IncreasesProductQuantity()
    {
        var result = await _stockEntryService.AddStockEntry(_product, 5, "IN");
        Assert.AreEqual(15, _product.Quantity);
        Assert.AreEqual("IN", result.EntryType);
    }

    [Test]
    public async Task AddStockEntry_OUT_DecreasesProductQuantity()
    {
        var result = await _stockEntryService.AddStockEntry(_product, 4, "OUT");
        Assert.AreEqual(6, _product.Quantity);
        Assert.AreEqual("OUT", result.EntryType);
    }

    [Test]
    public void AddStockEntry_InvalidEntryType_ThrowsException()
    {
        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            _stockEntryService.AddStockEntry(_product, 3, "INVALID"));

        Assert.That(ex.Message, Is.EqualTo("Entry type must be IN or OUT."));
    }

    [Test]
    public void AddStockEntry_OUTQuantityMoreThanStock_ThrowsException()
    {
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _stockEntryService.AddStockEntry(_product, 20, "OUT"));
    }
}
