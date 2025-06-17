using System.Text.Json;

namespace InventoryHub.Persistence
{
    public static class FileStorageHelper
    {
        public static async Task SaveToFileAsync<T>(string filePath, List<T> data)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }

        public static async Task<List<T>> LoadFromFileAsync<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }
    }
}
