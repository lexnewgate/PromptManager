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
    private readonly string _lastPathFile;

    public CardStorage()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dir = Path.Combine(appData, "PromptManager");
        Directory.CreateDirectory(dir);
        _filePath = Path.Combine(dir, "cards.json");
        _lastPathFile = Path.Combine(dir, "lastpath.txt");
    }

    /// <summary>
    /// 默认存储路径（无当前文件时保存使用的路径）。
    /// </summary>
    public string DefaultFilePath => _filePath;

    /// <summary>
    /// 获取上次打开的文件路径；不存在或为空则返回 null。
    /// </summary>
    public string? GetLastFilePath()
    {
        if (!File.Exists(_lastPathFile)) return null;
        try
        {
            var path = File.ReadAllText(_lastPathFile).Trim();
            return string.IsNullOrWhiteSpace(path) ? null : path;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 记录上次打开的文件路径（打开或加载成功后调用）。
    /// </summary>
    public void SetLastFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;
        try
        {
            File.WriteAllText(_lastPathFile, filePath);
        }
        catch { /* 忽略写入失败 */ }
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

    /// <summary>
    /// 从指定路径读取卡片 JSON 文件。
    /// </summary>
    public IReadOnlyList<Card> LoadFromPath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return new List<Card>();

        try
        {
            var json = File.ReadAllText(filePath);
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
