using System.Collections.Generic;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Isolation_Protocol.Models;

public partial class InventorySlot : ObservableObject
{
    [ObservableProperty]
    private Item? _item;

    [ObservableProperty]
    private int _count;

    [ObservableProperty]
    private bool _isSelected;
    
    [ObservableProperty]
    private bool _isEmpty = true;
    
    [ObservableProperty]
    private IImage? _image;

}