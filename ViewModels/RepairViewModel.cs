using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Models.Craft;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class RepairViewModel : ViewModelBase
{
    public Action StartRocket;
    
    public InventoryViewModel Inventory { get; } = new(6);
    private readonly Localization _loc = Localization.Instance;

    private readonly string[] _moduleKeys = { "Module_Engine", "Module_Hull" };
    private readonly string[] _reqKeys = { "Req_Engine_Detailed", "Req_Hull_Detailed" };
    
    public bool[] Repaired = { false, false };

    [ObservableProperty]
    private ObservableCollection<RepairModule> _module;
    
    [ObservableProperty]
    private string _errorMessage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentModuleName))]
    [NotifyPropertyChangedFor(nameof(RequirementsText))]
    [NotifyPropertyChangedFor(nameof(StatusText))]
    [NotifyPropertyChangedFor(nameof(IsCurrentModuleRepaired))]
    private int _selectedIndex = 0;
    
    private Rocket _rocket;

    public string CurrentModuleName => _loc[_moduleKeys[SelectedIndex]];
    public string StatusText => Repaired[SelectedIndex] ? "READY" : _loc["Repair_Critical"];
    public string RequirementsText => Repaired[SelectedIndex] ? _loc["Repair_Ready"] : _loc[_reqKeys[SelectedIndex]];
    public bool IsCurrentModuleRepaired => Repaired[SelectedIndex];
    public bool CanLaunch => Module.All(m => m.IsRepaired);
    
    private InventoryViewModel _inventory;

    public RepairViewModel(InventoryViewModel inv)
    {
        _inventory = inv;
        _module = new ObservableCollection<RepairModule>
        {
            new("Module_Engine", new() { new("motor", 1), new("emerald", 10), new("gold", 10) }),
            new("Module_Hull", new() { new("iron_plate", 7), new("gold_plate", 3), new("wood", 20) })
        };

        PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(CanLaunch))
            {
                ChanceRocketSprite();
            }
        };
    }

    [RelayCommand]
    public void Repair()
    {
        var currentModule = _module[SelectedIndex];

        foreach (CraftIngredient ingredient in currentModule.Ingredients)
        {
            if (_inventory.GetItemCount(ingredient.ItemTag) < ingredient.Amount)
            {
                ErrorMessage = Localization.Instance["Craft_Error_NoResources"];
                return;
            }
        }
        
        foreach (CraftIngredient ingredient in currentModule.Ingredients)
        {
            _inventory.RemoveItem(ingredient.ItemTag, ingredient.Amount);
        }
        
        ErrorMessage = string.Empty;
        _module[SelectedIndex].IsRepaired = true;
        Repaired[SelectedIndex] = true;
        
        OnPropertyChanged(nameof(StatusText));
        OnPropertyChanged(nameof(RequirementsText));
        OnPropertyChanged(nameof(IsCurrentModuleRepaired));
        OnPropertyChanged(nameof(CanLaunch));
    }
    
    [RelayCommand]
    public void LaunchRocket()
    {
        StartRocket.Invoke();
    }

    public void SetRocket(Rocket rocket)
    {
        _rocket = rocket;
    }

    public void ChanceRocketSprite()
    {
        if (_rocket.VisualElement.Parent is Canvas parent)
        {
            parent.Children.Remove(_rocket.VisualElement);
            var image = new Image() { Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Isolation Protocol/Assets/roket.png"))), Width = 40 * 1.5, Height = 40 * 2, ZIndex = 6};
            
            Canvas.SetLeft(image, _rocket.X * 40);
            Canvas.SetTop(image, _rocket.Y * 40);
            _rocket.VisualElement = image;
            parent.Children.Add(_rocket.VisualElement);
        }
    }
}