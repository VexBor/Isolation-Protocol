using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Isolation_Protocol.Models;

public class StoneItem : Item
{
    public StoneItem()
    {
        ItemType = ItemType.Resource;
        Name = "Камінь";
        Tag = "stone";
        MaxStack = 32;
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/StoneItem.png")));
    }
}