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
        TextureId = "Stone1";
        MaxHealth = 100f;
        Drop = 
        [
            new ResourceDrop(ItemRegistry.CreateItem("stone"), 2, 5)
        ];
    }
    
    public override bool OnInteract(Item? tool)
    {
        if (tool.Tag.Contains("pickaxe")) 
        {
            Sound.PlaySfx("stone");
            Health -= tool.Damage;
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40), 40, (Health / MaxHealth));
            if(Health <= 0) return true;
        }
        return false;
    }
}