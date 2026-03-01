using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using PromptManager.Models;

namespace PromptManager.Services;

public class CardStorage
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
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
        Save(cards, _filePath);
    }

    /// <summary>
    /// 将卡片保存到指定路径。
    /// </summary>
    public void Save(IEnumerable<Card> cards, string filePath)
    {
        var list = cards.ToList();
        var json = JsonSerializer.Serialize(list, JsonOptions);
        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllText(filePath, json);
    }
}
