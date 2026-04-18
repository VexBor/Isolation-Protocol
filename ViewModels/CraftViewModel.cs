using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Models.Craft;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class CraftViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<CraftRecipe> _recipes = new();

    [ObservableProperty]
    private CraftRecipe? _selectedRecipe;
    
    [ObservableProperty]
    private string? _errorMessage;
    
    private InventoryViewModel _inventory;
    private GameMap _map;

    public CraftViewModel(InventoryViewModel inventoryViewModel, GameMap map)
    {
        _inventory = inventoryViewModel;
        _map = map;
        LoadRecipes();
    }

    private void LoadRecipes()
    {
        var uri = new Uri("avares://Isolation Protocol/Assets/File/recipes.json");
    
        using var stream = AssetLoader.Open(uri);
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        var data = JsonSerializer.Deserialize<List<CraftRecipe>>(json);

        if (data != null) Recipes = data;
    }

    [RelayCommand]
    private void CraftItem()
    {
        Sound.PlaySfx("click");
        if (SelectedRecipe == null) return;
        if(!_inventory.EmptySlots(_selectedRecipe.ResultItemTag))
        {
            ErrorMessage = ErrorMessage = Localization.Instance["Craft_Error_NoSpace"];
            return;
        }

        if (SelectedRecipe.NeedWorkBench)
        {
            if (!_map.ObjectInVision("workbench"))
            {
                 ErrorMessage = Localization.Instance["Craft_Error_NeedWorkBench"];
                 return;
            }
        }

        foreach (CraftIngredient ingredient in _selectedRecipe.Ingredients)
        {
            if (_inventory.GetItemCount(ingredient.ItemTag) < ingredient.Amount)
            {
               ErrorMessage = Localization.Instance["Craft_Error_NoResources"];
                return;
            }
        }

        foreach (CraftIngredient ingredient in _selectedRecipe.Ingredients)
        {
            _inventory.RemoveItem(ingredient.ItemTag, ingredient.Amount);
        }
        
        ErrorMessage = string.Empty;
        _inventory.AddItem(ItemRegistry.CreateItem(SelectedRecipe.ResultItemTag),1);

    }
}