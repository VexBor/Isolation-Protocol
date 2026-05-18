using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Models.Craft;
using Isolation_Protocol.Services;
using SkiaSharp;

namespace Isolation_Protocol.View;

public partial class AnvilViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _errorMessage;
    
    private readonly InventoryViewModel _inventory;
    private readonly Localization _loc = Localization.Instance;

    private readonly string[] _recipeKeys = { "Craft_pickaxe_iron", "Craft_iron_plate", "Craft_gold_plate"};
    private readonly string[] _descKeys = { "Desc_Pickaxe", "Desc_Plate", "Desc_Gold_plate" };
    private readonly string[] _reqKeys = { "Req_Anvil_Pickaxe", "Req_Anvil_Plate",  "Req_Anvil_Gold_plate" };

    private readonly string _craftPath = "avares://Isolation Protocol/Assets/File/anvilRecipes.json";
        
    private readonly string[] _imagePaths = {
        "avares://Isolation Protocol/Assets/pickaxeIron.png",
        "avares://Isolation Protocol/Assets/ironPlate.png",
        "avares://Isolation Protocol/Assets/goldPlate.png"
    };
    
    private List<CraftRecipe> _recipes;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentItemName))]
    [NotifyPropertyChangedFor(nameof(ItemDescription))]
    [NotifyPropertyChangedFor(nameof(RequirementsText))]
    [NotifyPropertyChangedFor(nameof(CurrentItemImage))]
    private int _selectedIndex = 0;

    public string HeaderText => _loc["Anvil_Header"];
    public string[] RecipeDisplayNames => _recipeKeys.Select(k => _loc[k]).ToArray();
    public string CurrentItemName => GetSafeLoc(_recipeKeys);
    public string ItemDescription => GetSafeLoc(_descKeys);
    public string RequirementsText => GetSafeLoc(_reqKeys);
    
    public Bitmap? CurrentItemImage
    {
        get
        {
            if (SelectedIndex < 0 || SelectedIndex >= _imagePaths.Length) return null;
            try
            {
                return new Bitmap(AssetLoader.Open(new Uri(_imagePaths[SelectedIndex])));
            }
            catch
            {
                return null;
            }
        }
    }

    public AnvilViewModel(InventoryViewModel inv)
    {
        _inventory = inv;
        LoadRecipes();
    }

    [RelayCommand]
    private void Craft()
    {
        if (SelectedIndex < 0) return;

        var craftItem = _recipeKeys[SelectedIndex];

        foreach (var recipe in _recipes)
        {
            if (craftItem.Contains(recipe.ResultItemTag))
            {
                if(!_inventory.EmptySlots(recipe.ResultItemTag))
                {
                    ErrorMessage = ErrorMessage = Localization.Instance["Craft_Error_NoSpace"];
                    return;
                }

                foreach (CraftIngredient ingredient in recipe.Ingredients)
                {
                    if (_inventory.GetItemCount(ingredient.ItemTag) < ingredient.Amount)
                    {
                        ErrorMessage = Localization.Instance["Craft_Error_NoResources"];
                        return;
                    }
                }

                foreach (CraftIngredient ingredient in recipe.Ingredients)
                {
                    _inventory.RemoveItem(ingredient.ItemTag, ingredient.Amount);
                }
        
                ErrorMessage = string.Empty;
                _inventory.AddItem(ItemRegistry.CreateItem(recipe.ResultItemTag),1);
            }
        }
        
        Sound.PlaySfx("anvil_clank");
        
    }
    
    private void LoadRecipes()
    {
        var uri = new Uri(_craftPath);
    
        using var stream = AssetLoader.Open(uri);
        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        var data = JsonSerializer.Deserialize<List<CraftRecipe>>(json);

        if (data != null) _recipes = data;
    }

    private string GetSafeLoc(string[] keys) => 
        (SelectedIndex >= 0 && SelectedIndex < keys.Length) ? _loc[keys[SelectedIndex]] : "";
}