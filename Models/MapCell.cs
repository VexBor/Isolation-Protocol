using System.Collections.Generic;

namespace Isolation_Protocol.Models;

public enum CellType { Wall, Floor, Door, Trap, Exit }

public class MapCell
{
    public CellType Type { get; set; }
    public bool IsExplored { get; set; }
    public bool IsWalkable => Type != CellType.Wall;
    public List<string> Items { get; set; } = new();

    public MapCell(CellType type)
    {
        Type = type;
        IsExplored = false;
    }
}