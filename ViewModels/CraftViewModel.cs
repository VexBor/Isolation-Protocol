using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models.Craft;

namespace Isolation_Protocol.View;

public partial class CraftViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<CraftRecipe> _recipes = new();

    [ObservableProperty]
    private CraftRecipe? _selectedRecipe;

    public CraftViewModel()
    {
        LoadRecipes();
    }

    private void LoadRecipes()
    {
        try
        { 
            var uri = new Uri("avares://Isolation Protocol/Assets/File/recipes.json");
    
            using var stream = AssetLoader.Open(uri);
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var data = JsonSerializer.Deserialize<List<CraftRecipe>>(json);

            if (data != null)
            {
                Recipes = data;
                Console.Write(data);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка завантаження рецептів: {ex.Message}");
        }
    }

    [RelayCommand]
    private void CraftItem()
    {
        if (SelectedRecipe == null) return;
    }
}