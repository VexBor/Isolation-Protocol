using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class SettingsViewModel: ViewModelBase
{ 
    public Settings? Settings { get; set; }
    
    private string _settingsFilePath = "Assets/settings.json";

    public SettingsViewModel()
    {
        if (File.Exists(_settingsFilePath))
        {
            var str = File.ReadAllText(_settingsFilePath);
            Settings = JsonSerializer.Deserialize<Settings>(str);
        }
        else
            Settings = new Settings();
        ApplyLanguage(Settings.LanguageId);
    }

    private void ApplyLanguage(int index)
    {
        string langCode = index switch
        {
            0 => "en",
            1 => "ua",
            _ => "ua"
        };

        Localization.Instance.LoadLanguage(langCode);
    }
    
    [RelayCommand]
    private void SaveSettings()
    {
        Sound.PlaySfx("click");
        ApplyLanguage(Settings.LanguageId);
        var json = JsonSerializer.Serialize(Settings);
        File.WriteAllText(_settingsFilePath, json);
    }
}