using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Isolation_Protocol.Models;

public class Pickaxe : Item
{
    public Pickaxe()
    {
        Name = "Кайло";
        Tag = "pickaxe";
        ItemType = ItemType.Tool;
        Damage = 10;
        Durability = 100;
        MaxStack = 1;
        Image =  new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/pickaxe.png")));
    }
}