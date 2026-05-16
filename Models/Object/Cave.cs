using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;

namespace Isolation_Protocol.Models;

public class Cave : MapObject, IInteractable
{
    public Cave()
    {
        IsPassable = false;
        Tag = "cave";
        Health = -1f;
        TextureId = "cave";
        MaxHealth = -1f;
    }
    
    public bool OnInteract(Item? tool)
    {
        return false;
    }
}