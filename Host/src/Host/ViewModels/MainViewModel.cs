using System.Collections.Generic;

namespace Host.ViewModels;

public enum NavigationTarget
{
    Page1,
    Page2
}

public sealed class NavMenuItem
{
    public required string Title { get; init; }
    public required string Icon { get; init; }
    public required NavigationTarget Target { get; init; }
}

public sealed class MainViewModel : ViewModelBase
{
    private bool _navDrawerSwitch = true;
    private NavMenuItem? _selectedMenuItem;

    public IReadOnlyList<NavMenuItem> MenuItems { get; } =
    [
        new NavMenuItem { Title = "菜单1", Icon = "M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2M12,4A8,8 0 0,1 20,12A8,8 0 0,1 12,20A8,8 0 0,1 4,12A8,8 0 0,1 12,4M12,6A6,6 0 0,0 6,12A6,6 0 0,0 12,18A6,6 0 0,0 18,12A6,6 0 0,0 12,6M12,8A4,4 0 0,1 16,12A4,4 0 0,1 12,16A4,4 0 0,1 8,12A4,4 0 0,1 12,8Z", Target = NavigationTarget.Page1 },
        new NavMenuItem { Title = "菜单2", Icon = "M22,21H2V3H4V19H6V10H10V19H12V6H16V19H18V14H22V21Z", Target = NavigationTarget.Page2 },
    ];

    public MainViewModel()
    {
        SelectedMenuItem = MenuItems[0];
    }

    public NavMenuItem? SelectedMenuItem
    {
        get => _selectedMenuItem;
        set => SetProperty(ref _selectedMenuItem, value);
    }

    public bool NavDrawerSwitch
    {
        get => _navDrawerSwitch;
        set
        {
            if (SetProperty(ref _navDrawerSwitch, value))
            {
                OnPropertyChanged(nameof(IsMenuTextVisible));
            }
        }
    }

    public bool IsMenuTextVisible => NavDrawerSwitch;
}
