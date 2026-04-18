using System.Reactive.PlatformServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;
using Isolation_Protocol.Views;

namespace Isolation_Protocol.View;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;

    public MainWindowViewModel()
    {
        var st = new SettingsViewModel();
        CurrentPage = new LoginViewModel(this);
    }
    
    [RelayCommand]
    private void OpenMenu()
    {
        Sound.PlaySfx("click");
        CurrentPage = new MenuViewModel(this);
    }
    
    [RelayCommand]
    public void ExitToMenu()
    {
        Sound.PlaySfx("click");
        CurrentPage = new MenuViewModel(this);
    }
}