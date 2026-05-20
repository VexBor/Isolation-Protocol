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

            foreach (var drop in source.Drop)
            {
                int amount = Random.Shared.Next(drop.MinAmount, drop.MaxAmount + 1);
                if (amount > 0)
                {
                    _inventory.AddItem(drop.DropItem, amount);
                }
            }
        }
    }
}