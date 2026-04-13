using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Isolation_Protocol.Models;

public enum CellType { Wall, Floor, Door, Trap, Exit, Water, Sand}

public partial class MapCell : ObservableObject
{
    public CellType Type { get; set; }
    public bool IsWalkable => Type != CellType.Wall;
    
    [ObservableProperty]
    private MapObject? _object;
    public int X { get; set; }
    public int Y { get; set; }

    public MapCell(CellType type)
    {
        Type = type;
    }
    
}