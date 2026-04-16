using System.Reactive.PlatformServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Views;

namespace Isolation_Protocol.View;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase _currentPage;

    public MainWindowViewModel()
    {
        CurrentPage = new MenuViewModel(this);
    }
    
    [RelayCommand]
    private void OpenMenu()
    {
        CurrentPage = new MenuViewModel(this);
    }
    
    [RelayCommand]
    public void ExitToMenu()
    {
        CurrentPage = new MenuViewModel(this);
    }
}