using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class EscapeMenuViewModel(MapViewModel vm) :  ViewModelBase
{
    [ObservableProperty]
    private bool _isPaused;

    [RelayCommand]
    public void TogglePause()
    {
        Sound.PlaySfx("click");
        IsPaused = !IsPaused;
    }

    [RelayCommand]
    public void Resume()
    {
        IsPaused = false;
        Logs.Add("player resume game");
    } 

    [RelayCommand]
    public void QuitGame()
    {
        vm.SaveGame();
        Logs.Add("player quit game");
        Logs.Save();
        Environment.Exit(0);
    } 
}