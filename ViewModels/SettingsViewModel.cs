using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.View;

public partial class SettingsViewModel: ViewModelBase
{ 
    public Settings? Settings { get; set; }
    
    private string _settingsFilePath = "settings.json";

    public SettingsViewModel()
    {
        if (File.Exists(_settingsFilePath))
        {
            var str = File.ReadAllText(_settingsFilePath);
            Settings = JsonSerializer.Deserialize<Settings>(str);
        }
        else
            Settings = new Settings();
    }

    [RelayCommand]
    private void SaveSettings()
    {
        var json = JsonSerializer.Serialize(Settings);
        File.WriteAllText(_settingsFilePath, json);
    }
}