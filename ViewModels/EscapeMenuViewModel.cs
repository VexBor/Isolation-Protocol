using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Isolation_Protocol.View;

public partial class EscapeMenuViewModel :  ObservableObject
{
    [ObservableProperty]
    private bool _isPaused;

    [RelayCommand]
    public void TogglePause()
    {
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