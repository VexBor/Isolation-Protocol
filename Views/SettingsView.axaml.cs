using System;
using System.IO;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.Views;

public partial class SettingsView : UserControl
{
    private readonly string _settingsPath = "Data/settings.json";
    private Settings? settings;
    public SettingsView()
    {
        InitializeComponent();

        if (File.Exists(_settingsPath))
        {
            var json = File.ReadAllText(_settingsPath);
            settings = JsonSerializer.Deserialize<Settings>(json);
            
            musicSlider.Value = settings.MusicVolume;
            volumeSlider.Value = settings.Volume;
            
            for (int i = 0; i < languageComboBox.Items.Count; i++)
            {
                if (languageComboBox.Items[i] is ComboBoxItem item && item.Tag?.ToString() == settings.Language)
                {
                    languageComboBox.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    private void Save(object? sender, RoutedEventArgs e)
    {
        string selectedItem = "uk";
        if (languageComboBox.SelectedItem is ComboBoxItem item)
        {
            selectedItem = item?.Tag?.ToString() ?? "uk";
        }
        
        var newSettings = new Settings
        {
            Volume = (int)volumeSlider.Value,
            MusicVolume = (int)musicSlider.Value,
            Language = selectedItem
        };

        string json = JsonSerializer.Serialize(newSettings);

        File.WriteAllText(_settingsPath, json);
    }
}