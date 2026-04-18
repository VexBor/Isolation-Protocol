using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;

namespace Isolation_Protocol.Models;

public class Workbench : MapObject, IInteractable
{
    public Workbench()
    {
        IsPassable = false;
        Tag = "workbench";
        Health = 500;
        Drop = new ResourceDrop(ItemRegistry.CreateItem("workbench"), 1, 1);
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/workbench.png")));
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