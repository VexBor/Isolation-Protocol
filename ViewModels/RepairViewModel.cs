using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Models.Craft;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class RepairViewModel : ViewModelBase
{
    public InventoryViewModel Inventory { get; } = new(6);
    private readonly Localization _loc = Localization.Instance;

    private readonly string[] _moduleKeys = { "Module_Engine", "Module_Hull" };
    private readonly string[] _reqKeys = { "Req_Engine_Detailed", "Req_Hull_Detailed" };
    private readonly bool[] _repaired = { false, false };

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

    public string CurrentModuleName => _loc[_moduleKeys[SelectedIndex]];
    public string StatusText => _repaired[SelectedIndex] ? "READY" : _loc["Repair_Critical"];
    public string RequirementsText => _repaired[SelectedIndex] ? _loc["Repair_Ready"] : _loc[_reqKeys[SelectedIndex]];
    public bool IsCurrentModuleRepaired => _repaired[SelectedIndex];
    
    private InventoryViewModel _inventory;

    public RepairViewModel(InventoryViewModel inv)
    {
        _inventory = inv;
        _module = new ObservableCollection<RepairModule>
        {
            new("Module_Engine", new() { new("motor", 1), new("emerald", 10), new("gold", 10) }),
            new("Module_Hull", new() { new("iron_plate", 7), new("gold_plate", 3), new("wood", 20) })
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
        _repaired[SelectedIndex] = true;
        
        OnPropertyChanged(nameof(StatusText));
        OnPropertyChanged(nameof(RequirementsText));
        OnPropertyChanged(nameof(IsCurrentModuleRepaired));
    }
}