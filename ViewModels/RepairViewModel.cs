using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Isolation_Protocol.View;

public partial class RepairViewModel : ViewModelBase
{
    public InventoryViewModel Inventory { get; } = new(6);

    [ObservableProperty] 
    private ObservableCollection<string> _moduleName = new ObservableCollection<string>()
    {
        "Module1",
        "Module2",
        "Module3"
    };

    [RelayCommand]
    public void Repair()
    {
        ModuleName.Add("Module4");
    }
}