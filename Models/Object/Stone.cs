using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models;

public class Stone : MapObject,  IInteractable
{
    public Stone()
    {
        IsPassable = false;
        Tag = "stone";
        Health = 100f;
        MaxHealth = 100f;
        Drop = new ResourceDrop(ItemRegistry.CreateItem("stone"), 2, 5);
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/Stone1.png")));
    }
    
    public bool OnInteract(Item? tool)
    {
        if (tool.Tag == "pickaxe") 
        {
            Sound.PlaySfx("stone");
            Health -= tool.Damage;
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40), 40, (Health / MaxHealth));
            if(Health <= 0) return true;
        }
        return false;
    }
}