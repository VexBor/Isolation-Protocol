using Avalonia.Media;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models;

public enum ItemType
{
    Tool,
    Resource
}

public class Item
{
    public string Name => Localization.Instance[$"Item_{Tag}_Name"];
    public string Tag { get; set; }
    public string? Description { get; set; }
    public IImage? Image { get; set; }
    public ItemType ItemType { get; set; }
    public int Damage { get; set; }
    public int Durability { get; set; }
    public int MaxStack { get; set; }
    
    public Item Clone() => (Item)this.MemberwiseClone();
}