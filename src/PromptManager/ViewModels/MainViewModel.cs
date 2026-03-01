using System.Collections.ObjectModel;
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

    public MainViewModel()
    {
        Load();
    }

    public void Load()
    {
        var list = _storage.Load();
        Cards.Clear();
        foreach (var card in list)
            Cards.Add(card);
    }

    public void Save()
    {
        _storage.Save(Cards);
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
