using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class DevPanelViewModel : ViewModelBase
{
    public ObservableCollection<string> AvailableItems { get; } = new ObservableCollection<string>(ItemRegistry.GetAllItemTag());

    [ObservableProperty]
    private string? _selectedItem;

    [ObservableProperty]
    private int _spawnAmount = 1;

    [ObservableProperty] 
    private string _systemLogs = Logs.Get();

    private InventoryViewModel _inventory;
    private Player _player;
    
    public DevPanelViewModel(InventoryViewModel inventory, Player player)
    {
        SelectedItem = AvailableItems[0];
        _inventory = inventory;
        _player = player;
    }

    [RelayCommand]
    private void SpawnItem()
    {
        if (string.IsNullOrEmpty(SelectedItem) || SpawnAmount <= 0) return;
        _inventory.AddItem(ItemRegistry.CreateItem(SelectedItem), SpawnAmount);
        Logs.Add($"give {SpawnAmount} {SelectedItem}");
    }

    [RelayCommand]
    private void RestoreStats()
    {
        _player.Health = 100;
        _player.Stamina = 100;
        _player.Hunger = 100;
        Logs.Add("stats restored");
    }
}
