using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Isolation_Protocol.Models;

public class Tree : MapObject
{
    public Tree()
    {
        Name = "Tree";
        IsPassable = false;
        Health = 100;
        Image = new Bitmap(AssetLoader.Open(
            new Uri($"avares://{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}/Assets/tree.png")));
    }
}