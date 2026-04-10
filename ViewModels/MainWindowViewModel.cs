using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Views;

namespace Isolation_Protocol.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;
    
    [ObservableProperty]
    private bool _isGameStarted = false;
    
    
    public MainWindowViewModel()
    {
        CurrentPage = new HeroViewModel();
    }

    [RelayCommand]
    private void OpenSettings()
    {
        CurrentPage = new SettingsViewModel();
    }

    [RelayCommand]
    private void OpenInventory()
    {
        CurrentPage = new InventoryViewModel();
    }

    [RelayCommand]
    private void OpenHeroMenu()
    {
        CurrentPage = new HeroViewModel();
    }
    
    [RelayCommand]
    private void OpenInfo()
    {
        CurrentPage = new InfoViewModel();
    }

    [RelayCommand]
    private void StartGame()
    {
        CurrentPage = new MapViewModel();
        IsGameStarted = true;
    }
}