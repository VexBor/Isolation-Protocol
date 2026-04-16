using System;
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

    public MenuViewModel(MainWindowViewModel mainVM)
    {
        _mainVM = mainVM;
    }

    [RelayCommand]
    private void StartGame()
    {
        _mainVM.CurrentPage = new MapViewModel();
    }
    
    [RelayCommand]
    private void OpenSettings()
    {
        _mainVM.CurrentPage = new SettingsViewModel();
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