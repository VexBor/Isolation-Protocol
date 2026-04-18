using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class EscapeMenuViewModel :  ViewModelBase
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
    } 

    [RelayCommand]
    public void QuitGame()
    {
        Environment.Exit(0);
    } 
}