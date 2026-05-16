using System;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Models;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Services;

public class DropLogic
{
    private readonly InventoryViewModel _inventory;

    public DropLogic(InventoryViewModel inventory) => _inventory = inventory;

    public void Drop(MapObject source)
    {
        if (source != null)
        {
            if(source.Drop == null) return;
            
            int amount = Random.Shared.Next(source.Drop.MinAmount, source.Drop.MaxAmount + 1);
            if (amount > 0)
            {
                _inventory.AddItem(source.Drop.DropItem, amount);
            }
        }
    }
}