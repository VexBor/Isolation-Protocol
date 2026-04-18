using System;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Isolation_Protocol.View;

public partial class MenuViewModel : ViewModelBase
{
    [ObservableProperty] private IImage _image = new Bitmap(AssetLoader.Open(
        new Uri(@"avares://Isolation Protocol/Assets/background.png")));
    
    private MainWindowViewModel _mainVM;
    private SettingsViewModel _settingsVM = new();

    public MenuViewModel(MainWindowViewModel mainVM)
    {
        _mainVM = mainVM;
    }

    [RelayCommand]
    private async void StartGame()
    {
        _mainVM.CurrentPage = new LoadingViewModel();

        MapViewModel game = null;
        
        await Task.Run(async () =>
        {
            game = new MapViewModel();
            await Task.Delay(1500);
        });
        _mainVM.CurrentPage = game;
    }
    
    [RelayCommand]
    private void OpenSettings()
    {
        _mainVM.CurrentPage = _settingsVM;
    }
        
    [RelayCommand]
    private void OpenInfo()
    {
        _mainVM.CurrentPage = new InfoViewModel();
    }
    
    [RelayCommand]
    private void Exit()
    {
        Environment.Exit(0);
    }
}