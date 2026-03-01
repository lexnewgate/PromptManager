using System.IO;
using System.Text.Json;
using PromptManager.Models;

namespace PromptManager.Services;

public class CardStorage
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly string _filePath;

    public CardStorage()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "PromptManager");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "cards.json");
    }

    public IReadOnlyList<Card> Load()
    {
        if (!File.Exists(_filePath))
            return new List<Card>();

        try
        {
            var json = File.ReadAllText(_filePath);
            var list = JsonSerializer.Deserialize<List<Card>>(json, JsonOptions);
            return list ?? new List<Card>();
        }
        catch
        {
            return new List<Card>();
        }
    }

    public void Save(IEnumerable<Card> cards)
    {
        var list = cards.ToList();
        var json = JsonSerializer.Serialize(list, JsonOptions);
        File.WriteAllText(_filePath, json);
    }
}
