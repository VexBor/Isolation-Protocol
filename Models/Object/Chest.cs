using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Services;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Models;

public class Chest : MapObject, IInteractable
{
    public InventoryViewModel ChestInventory { get; }
    
    public Chest()
    {
        Name = "Скриня";
        IsPassable = false;
        Tag = "chest";
        Health = 500;
        Drop = new ResourceDrop(ItemRegistry.CreateItem("chest")!, 1, 1);
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/chest.png")));
        ChestInventory = new InventoryViewModel(9);
    }
    
    public bool OnInteract(Item? tool)
    {
        if (tool.Tag == "axe") 
        {
            Health -= tool.Damage;
            if (Health <= 0)
            {
                Health = 500;
                return true;
            }
        }
        return false;
    }
}