using Avalonia.Controls;
using Host.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace Host.Views;

public partial class MainView : UserControl
{
    private readonly SemaphoreSlim _navigationLock = new(1, 1);
    private readonly Page1ViewModel _page1ViewModel = new();
    private readonly Page2ViewModel _page2ViewModel = new();
    private NavigationTarget _activeTarget = NavigationTarget.Page1;

    public MainView()
    {
        InitializeComponent();
        MainNavigation.Content = CreatePage(NavigationTarget.Page1);
    }

    private async void OnMenuSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox { SelectedItem: NavMenuItem menuItem })
        {
            await ShowRootPageAsync(menuItem.Target);
        }
    }

    private async Task ShowRootPageAsync(NavigationTarget target)
    {
        await _navigationLock.WaitAsync();

        try
        {
            if (target == _activeTarget && MainNavigation.StackDepth == 1)
            {
                return;
            }

            if (MainNavigation.StackDepth > 1)
            {
                await MainNavigation.PopToRootAsync();
            }

            var page = CreatePage(target);

            if (MainNavigation.StackDepth == 0)
            {
                MainNavigation.Content = page;
            }
            else
            {
                await MainNavigation.ReplaceAsync(page);
            }

            _activeTarget = target;
        }
        finally
        {
            _navigationLock.Release();
        }
    }

    private ContentPage CreatePage(NavigationTarget target)
    {
        return target switch
        {
            NavigationTarget.Page1 => new Page1View { DataContext = _page1ViewModel },
            NavigationTarget.Page2 => new Page2View { DataContext = _page2ViewModel },
            _ => throw new System.ArgumentOutOfRangeException(
                nameof(target),
                target,
                "No page is registered for this navigation target.")
        };
    }
}
