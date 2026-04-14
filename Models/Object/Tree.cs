using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;

namespace Isolation_Protocol.Models;

public class Tree : MapObject, IInteractable
{
    public Tree()
    {
        Name = "Tree";
        IsPassable = false;
        Tag = "tree";
        Health = 100;
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/tree.png")));
        Drop = new ResourceDrop(ItemRegistry.CreateItem("wood"), 2, 5);
    }
    
    public bool OnInteract(Item? tool)
    {
        if (tool.Tag == "axe") 
        {
            Health -= tool.Damage;
            if(Health <= 0) return true;
        }
        return false;
    }
}