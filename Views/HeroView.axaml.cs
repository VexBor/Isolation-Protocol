using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.Views;

public partial class HeroView : UserControl
{
    private string _heroPath = "Data/heroes.json";
    private List<Hero>? heroes = new List<Hero>();
    private string? _assemblyName;

    public HeroView()
    {
        InitializeComponent();

        _assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

        if (File.Exists(_heroPath))
        {
            var str = File.ReadAllText(_heroPath);
            heroes = JsonSerializer.Deserialize<List<Hero>>(str);
        }
    }

    private void Select_Button(object? sender, RoutedEventArgs e)
    {
        if (sender is Button clickedButton)
        {
            var targetHero = heroes.FirstOrDefault(hero => hero.Name + "Button" == clickedButton.Name);

            if (targetHero != null)
            {
                var uri = new Uri($"avares://{_assemblyName}/{targetHero.ImagePath}");
                
                selectedHeroImage.Source = new Bitmap(AssetLoader.Open(uri));
                heroNameText.Text = targetHero.Name;
                heroHealthText.Text = $"HP: {targetHero.Health}";
                heroStaminaText.Text = $"Stamina: {targetHero.Stamina}";
                heroArmorText.Text = $"Armor: {targetHero.Armor}";
                heroStrengthText.Text = $"Strength: {targetHero.Strength}";
            }
        }
    }
}