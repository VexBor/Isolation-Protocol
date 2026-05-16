using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Interfaces;

namespace Isolation_Protocol.Models;

public class Rocket : MapObject, IInteractable
{
    public Rocket()
    {
        IsPassable = false;
        Tag = "rocket";
        Health = 500f;
        TextureId = "broken_roket";
        MaxHealth = 500f;
    }
    
    public bool OnInteract(Item? tool)
    {
        return false;
    }
}