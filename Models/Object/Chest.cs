using System;
using Avalonia;
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
        IsPassable = false;
        Tag = "chest";
        Health = 250f;
        MaxHealth = 250f;
        Drop = new ResourceDrop(ItemRegistry.CreateItem("chest")!, 1, 1);
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/chest.png")));
        ChestInventory = new InventoryViewModel(9);
    }
    
    public bool OnInteract(Item? tool)
    {
        if (tool.Tag == "axe") 
        {
            Sound.PlaySfx("tree");
            Health -= tool.Damage;
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40), 40, (Health / MaxHealth));
            if (Health <= 0)
            {
                return true;
            }
        }
        return false;
    }
}