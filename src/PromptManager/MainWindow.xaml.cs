using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using PromptManager.Models;
using PromptManager.ViewModels;
using PromptManager.Views;

namespace PromptManager;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        _vm = new MainViewModel();
        DataContext = _vm;
    }

    private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "打开卡片文件",
            Filter = "JSON 文件 (*.json)|*.json|所有文件 (*.*)|*.*"
        };
        if (dialog.ShowDialog() == true)
        {
            _vm.LoadFromPath(dialog.FileName);
        }
    }

    private void SaveAsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "另存为",
            Filter = "JSON 文件 (*.json)|*.json|所有文件 (*.*)|*.*",
            DefaultExt = ".json",
            FileName = "cards.json"
        };
        if (dialog.ShowDialog() == true)
        {
            _vm.SaveToPath(dialog.FileName);
        }
    }

    private void NewCardButton_OnClick(object sender, RoutedEventArgs e)
    {
        var newCard = new Card();
        var dialog = new CardEditDialog(newCard, isNew: true) { Owner = this };
        if (dialog.ShowDialog() == true)
        {
            _vm.AddCard(dialog.EditedCard);
        }
    }

    private void Card_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not FrameworkElement fe || fe.DataContext is not Card card)
            return;
        var copy = card.Clone();
        var dialog = new CardEditDialog(copy, isNew: false) { Owner = this };
        if (dialog.ShowDialog() == true)
        {
            _vm.UpdateCard(dialog.EditedCard);
        }
    }

    private static Card? GetCardFromMenuItem(object sender)
    {
        if (sender is not MenuItem menuItem)
            return null;

        if (menuItem.DataContext is Card cardFromDataContext)
            return cardFromDataContext;

        if (menuItem.Parent is ContextMenu contextMenu &&
            contextMenu.PlacementTarget is FrameworkElement fe &&
            fe.DataContext is Card cardFromPlacementTarget)
        {
            return cardFromPlacementTarget;
        }

        return null;
    }

    private void CardDeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var card = GetCardFromMenuItem(sender);
        if (card is null)
            return;

        var result = MessageBox.Show(
            $"确认删除卡片“{card.Title}”吗？",
            "确认删除",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result == MessageBoxResult.Yes)
        {
            _vm.RemoveCard(card);
        }
    }

    private void CardCopyMenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var card = GetCardFromMenuItem(sender);
        if (card is null)
            return;

        var text = card.Content ?? string.Empty;
        Clipboard.SetText(text);
    }
}
