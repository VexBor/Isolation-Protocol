using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Services;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Models;

public class Anvil : MapObject, IInteractable
{
    public Anvil()
    {
        IsPassable = false;
        Tag = "anvil";
        Health = 250f;
        TextureId = "anvil";
        MaxHealth = 250f;
        Drop =
        [
            new ResourceDrop(ItemRegistry.CreateItem("")!, 1, 1)
        ];
    }
    
    public override bool OnInteract(Item? tool)
    {
        if (tool.Tag == "pickaxe_iron") 
        {
            Sound.PlaySfx("stone");
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