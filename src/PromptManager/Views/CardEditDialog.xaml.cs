using System.Windows;
using PromptManager.Models;
using PromptManager.ViewModels;

namespace PromptManager.Views;

public partial class CardEditDialog : Window
{
    private readonly CardEditViewModel _vm;

    public Card EditedCard => _vm.SourceCard;

    public CardEditDialog(Card card, bool isNew)
    {
        InitializeComponent();
        _vm = new CardEditViewModel(card, isNew);
        DataContext = _vm;
        Title = isNew ? "新建卡片" : "编辑卡片";
    }

    private void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
