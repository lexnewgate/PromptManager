using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PromptManager.Models;
using PromptManager.Services;

namespace PromptManager.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly CardStorage _storage = new();

    [ObservableProperty]
    private ObservableCollection<Card> _cards = new();

    [ObservableProperty]
    private string _currentFilePath = string.Empty;

    public MainViewModel()
    {
        LoadLastOrShowEmpty();
    }

    /// <summary>
    /// 读取上次打开的文件：若存在则加载，否则显示空面板（不创建文件）。
    /// </summary>
    private void LoadLastOrShowEmpty()
    {
        var lastPath = _storage.GetLastFilePath();
        if (!string.IsNullOrWhiteSpace(lastPath) && File.Exists(lastPath))
        {
            var list = _storage.LoadFromPath(lastPath);
            Cards.Clear();
            foreach (var card in list)
                Cards.Add(card);
            CurrentFilePath = lastPath;
        }
        else
        {
            Cards.Clear();
            CurrentFilePath = string.Empty;
        }
    }

    public void Save()
    {
        if (!string.IsNullOrWhiteSpace(CurrentFilePath))
            _storage.Save(Cards, CurrentFilePath);
        else
            _storage.Save(Cards);
    }

    [RelayCommand]
    private void SaveToDisk()
    {
        Save();
    }

    /// <summary>
    /// 从指定路径打开卡片文件，替换当前列表。
    /// </summary>
    public void LoadFromPath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;
        var list = _storage.LoadFromPath(filePath);
        Cards.Clear();
        foreach (var card in list)
            Cards.Add(card);
        CurrentFilePath = filePath;
        _storage.SetLastFilePath(filePath);
    }

    /// <summary>
    /// 保存到指定路径（由“另存为”选择）。
    /// </summary>
    public void SaveToPath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;
        _storage.Save(Cards, filePath);
    }

    public void AddCard(Card card)
    {
        card.Id = Guid.NewGuid();
        card.CreatedAt = DateTime.UtcNow;
        card.UpdatedAt = DateTime.UtcNow;
        Cards.Add(card);
        Save();
    }

    public void UpdateCard(Card card)
    {
        card.UpdatedAt = DateTime.UtcNow;
        var index = Cards.ToList().FindIndex(c => c.Id == card.Id);
        if (index >= 0)
        {
            Cards[index] = card;
            Save();
        }
    }

    public void RemoveCard(Card card)
    {
        var item = Cards.FirstOrDefault(c => c.Id == card.Id);
        if (item != null)
        {
            Cards.Remove(item);
            Save();
        }
    }
}
