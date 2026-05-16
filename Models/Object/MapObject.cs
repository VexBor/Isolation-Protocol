using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.View;
using Newtonsoft.Json;

namespace Isolation_Protocol.Models;

public class MapObject : IInteractable
{
    public string? Tag { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public bool IsPassable { get; set; }
    public string TextureId { get; set; }
    [JsonIgnore] public Image? VisualElement { get; set; }
    public ResourceDrop? Drop { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public InventoryViewModel? ChestInventory { get; set; }
    
    public virtual bool OnInteract(Item? tool)
    {
        return false;
    }
}