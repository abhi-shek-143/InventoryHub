# InventoryHub

InventoryHub is a console-based inventory management system built with C#. It allows users to manage products, suppliers, and stock entries with full CRUD support, JSON file persistence, and unit testing.

## Features

- Add, update, delete, and list products
- Manage suppliers and stock entries
- Group products by supplier
- Filter low-stock products
- Save/load inventory data using JSON files
- Input validation and exception handling
- Unit tested using NUnit

## Technologies

- C# (.NET)
- Visual Studio
- NUnit for unit testing
- JSON file storage

## Project Structure

InventoryHub/
├── Models/
├── Services/
├── Persistence/
├── Program.cs
├── InventoryHub.csproj

InventoryHub.Tests/
├── ProductServiceTests.cs
├── SupplierServiceTests.cs
├── StockEntryServiceTests.cs
└── InventoryHub.Tests.csproj


## How to Run

1. Open the solution in Visual Studio.
2. Set `InventoryHub` as the startup project.
3. Press `Ctrl + F5` to run the application.

## Run Unit Tests

- Open **Test Explorer** in Visual Studio
- Click "Run All" to execute tests


