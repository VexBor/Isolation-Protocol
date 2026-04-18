using CommunityToolkit.Mvvm.ComponentModel;

namespace Isolation_Protocol.Models;

public partial class Settings : ObservableObject
{
    [ObservableProperty]
    private float _volume = 1f;
    
    [ObservableProperty]
    private int _languageId = 6;
}