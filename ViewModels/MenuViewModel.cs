using System;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class MenuViewModel : ViewModelBase
{
    [ObservableProperty] private IImage _image = new Bitmap(AssetLoader.Open(
        new Uri(@"avares://Isolation Protocol/Assets/background.png")));

    [ObservableProperty] private string _agentName = Authorize.CurrentUser.Username;
    [ObservableProperty] private string _agentId = "ID: " + Authorize.CurrentUser.Id;
    
    private MainWindowViewModel _mainVM;
    private SettingsViewModel _settingsVM = new();

    public MenuViewModel(MainWindowViewModel mainVM)
    {
        _mainVM = mainVM;
        Logs.Init();
    }

    [RelayCommand]
    private async void StartGame()
    {
        Sound.PlaySfx("click");
        _mainVM.CurrentPage = new LoadingViewModel();
        Logs.Add("player start game");
        Logs.Save();
        
        MapViewModel game = null;
        
        await Task.Run(async () =>
        {
            game = new MapViewModel();
            game.LoadGame();
            await Task.Delay(2000);
        });
        _mainVM.CurrentPage = game;
    }
    
    [RelayCommand]
    private async void StartNewGame()
    {
        Sound.PlaySfx("click");
        _mainVM.CurrentPage = new LoadingViewModel();
        Logs.Add("player start new game");
        Logs.Save();

        MapViewModel game = null;
        
        await Task.Run(async () =>
        {
            game = new MapViewModel();
            game.NewGame();
            await Task.Delay(2000);
        });
        _mainVM.CurrentPage = game;
    }
    
    [RelayCommand]
    private void OpenSettings()
    {
        Logs.Add("player open settings");
        Logs.Save();
        Sound.PlaySfx("click");
        _mainVM.CurrentPage = _settingsVM;
    }
    
    [RelayCommand]
    private void OpenGuide()
    {
        Logs.Add("player open guide");
        Logs.Save();
        Sound.PlaySfx("click");
        _mainVM.CurrentPage = new GuideViewModel();
    }
        
    [RelayCommand]
    private void OpenInfo()
    {
        Logs.Add("player open info");
        Logs.Save();
        Sound.PlaySfx("click");
        _mainVM.CurrentPage = new InfoViewModel();
    }
    
    [RelayCommand]
    private void Logout()
    {
        Sound.PlaySfx("click");
        Logs.Add("player logout");
        Authorize.CurrentUser = null;
        _mainVM.CurrentPage = new LoginViewModel(_mainVM);
    }
    
    [RelayCommand]
    private void Exit()
    {
        Sound.PlaySfx("click");
        Logs.Add("player exit game");
        Logs.Save();
        Environment.Exit(0);
    }
}