using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models;

public class Tree : MapObject, IInteractable
{
    public Tree()
    {
        IsPassable = false;
        Tag = "tree";
        TextureId = "tree";
        Health = 100f;
        MaxHealth = 100f;
        Drop =
        [
            new ResourceDrop(ItemRegistry.CreateItem("wood"), 2, 5),
            new ResourceDrop(ItemRegistry.CreateItem("apple"), 0, 2)
        ];
    }
    
    public override bool OnInteract(Item? tool)
    {
        if (tool.Tag.Contains(tool.Tag)) 
        {
            Sound.PlaySfx("tree");
            Health -= tool.Damage;
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40 - 20), 40, (Health / MaxHealth));
            if(Health <= 0) return true;
        }
        return false;
    }
}