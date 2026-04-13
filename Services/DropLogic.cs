using System;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Models;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Services;

public class DropLogic
{
    private readonly InventoryViewModel _inventory;

    public DropLogic(InventoryViewModel inventory) => _inventory = inventory;

    public void Drop(IInteractable source)
    {
        if (source is MapObject obj)
        {
            if(obj.Drop == null) return;
            
            int amount = Random.Shared.Next(obj.Drop.MinAmount, obj.Drop.MaxAmount + 1);
            if (amount > 0)
            {
                _inventory.AddItem(obj.Drop.DropItem, amount);
            }
        }
    }
}