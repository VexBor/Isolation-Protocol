using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Isolation_Protocol.Models;

public class Workbench : MapObject
{
    public Workbench()
    {
        Name = "Workbench";
        IsPassable = false;
        Tag = "workbench";
        Health = 100;
        Image = new Bitmap(AssetLoader.Open(new Uri("avares://Isolation Protocol/Assets/workbench.png")));
    }
}