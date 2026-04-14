using Avalonia.Media;

namespace Isolation_Protocol.Models;

public enum ItemType
{
    Tool,
    Resource
}

public class Item
{
    public string Name { get; set; }
    public string Tag { get; set; }
    public string? Description { get; set; }
    public IImage? Image { get; set; }
    public ItemType ItemType { get; set; }
    public int Damage { get; set; }
    public int Durability { get; set; }
    public int MaxStack { get; set; }
    public MapObject? Object { get; set; }
    
    public Item Clone() => (Item)this.MemberwiseClone();
}