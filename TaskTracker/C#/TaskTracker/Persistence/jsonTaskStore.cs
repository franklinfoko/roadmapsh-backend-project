using System.Text.Json;
using TaskTracker.Models;

namespace TaskTracker.Persistence;

public class JsonTaskStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonTaskStore(string filePath) => _filePath = filePath;

    public Dictionary<string, Task> Load()
    {
        if (!File.Exists(_filePath)) return [];

        try
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<Dictionary<string, Task>>(json) ?? [];
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error loading tasks: {ex.Message}");
            File.WriteAllText(_filePath, "{}");
            return [];
        }
    }

    public void Save(Dictionary<string, Task> tasks)
    {
        string json = JsonSerializer.Serialize(tasks, _options);
        File.WriteAllText(_filePath, json);
    }
}
