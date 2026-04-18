using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Avalonia.Platform;

namespace Isolation_Protocol.Services;

public class Localization : INotifyPropertyChanged
{
    public static Localization Instance { get; } = new();
    
    private Dictionary<string, string> _translations = new();

    public string this[string key] => _translations.GetValueOrDefault(key, $"#{key}#");

    public void LoadLanguage(string langCode)
    {
        var uri = new Uri($"avares://Isolation Protocol/Assets/Lang/{langCode}.json");
    
        using var stream = AssetLoader.Open(uri);
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        
        _translations = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new();
        
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}