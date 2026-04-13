using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Isolation_Protocol.Models;

public class Wood : Item
{
    public Wood()
    {
        ItemType = ItemType.Resource;
        Name = "Древисина";
        Tag = "wood";
        MaxStack = 32;
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/wood.png")));
    }
}