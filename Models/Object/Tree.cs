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
        Health = 100f;
        MaxHealth = 100f;
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/tree.png")));
        Drop = new ResourceDrop(ItemRegistry.CreateItem("wood"), 2, 5);
    }
    
    public bool OnInteract(Item? tool)
    {
        if (tool.Tag == "axe") 
        {
            Sound.PlaySfx("tree");
            Health -= tool.Damage;
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40 - 20), 40, (Health / MaxHealth));
            if(Health <= 0) return true;
        }
        return false;
    }
}