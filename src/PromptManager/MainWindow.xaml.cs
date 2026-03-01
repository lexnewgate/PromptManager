using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

    private void AddCardButton_OnClick(object sender, RoutedEventArgs e)
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
}
