using Avalonia.Media;

namespace Isolation_Protocol.Models;

public class MapObject
{
    public string? Name { get; set; }
    public int Health { get; set; }
    public bool IsPassable { get; set; }
    public IImage? Image { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}