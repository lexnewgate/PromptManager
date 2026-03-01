using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PromptManager.Models;

namespace PromptManager.ViewModels;

public partial class CardEditViewModel : ObservableObject
{
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _summary = string.Empty;

    [ObservableProperty]
    private string _content = string.Empty;

    public Card SourceCard { get; }
    public bool IsNew { get; }

    public CardEditViewModel(Card card, bool isNew)
    {
        SourceCard = card;
        IsNew = isNew;
        Title = card.Title;
        Summary = card.Summary;
        Content = card.Content;
    }

    public void ApplyToCard()
    {
        SourceCard.Title = Title;
        SourceCard.Summary = Summary;
        SourceCard.Content = Content;
    }

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    private void Confirm() => ApplyToCard();

    private bool CanConfirm() => !string.IsNullOrWhiteSpace(Title);

    partial void OnTitleChanged(string value) => ConfirmCommand.NotifyCanExecuteChanged();
}
