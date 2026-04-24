using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models;

public class Workbench : MapObject, IInteractable
{
    public Workbench()
    {
        IsPassable = false;
        Tag = "workbench";
        Health = 250f;
        MaxHealth = 250f;
        Drop = new ResourceDrop(ItemRegistry.CreateItem("workbench"), 1, 1);
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/workbench.png")));
    }
    
    public bool OnInteract(Item? tool)
    {
        if (tool.Tag == "axe") 
        {
            Sound.PlaySfx("tree");
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40), 40, (Health / MaxHealth));
            Health -= tool.Damage;
            if (Health <= 0)
            {
                return true;
            }
        }
        return false;
    }
}