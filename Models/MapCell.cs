using System.Collections.Generic;

namespace Isolation_Protocol.Models;

public enum CellType { Wall, Floor, Door, Trap, Exit, Water, Sand}

public class MapCell
{
    public CellType Type { get; set; }
    public bool IsWalkable => Type != CellType.Wall;
    public MapObject? Object { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public MapCell(CellType type)
    {
        Type = type;
    }
    
}