using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Isolation_Protocol.Models;

public class Axe : Item
{
    public Axe()
    {
        Name = "Топор";
        Tag = "axe";
        ItemType = ItemType.Tool;
        Damage = 10;
        Durability = 100;
        MaxStack = 1;
        Image =  new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/axe.png")));
    }
}