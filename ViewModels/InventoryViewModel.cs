using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Avalonia.Controls;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.View;

public class InventoryViewModel: ViewModelBase
{
    public Player Player { get; set; }
    public List<InventorySlot> Slots { get; set; } = new List<InventorySlot>();
    public int SelectedSlot { get; set; }
    
    public InventoryViewModel(Player player)
    {
        Player = player;
        
        Slots.Add(new InventorySlot());
        Slots.Add(new InventorySlot());
        Slots.Add(new InventorySlot());
        Slots.Add(new InventorySlot());
        Slots.Add(new InventorySlot());
        Slots.Add(new InventorySlot());
        
        Slots[0].IsSelected = true;
        AddItem(ItemRegistry.CreateItem("axe"), 1);
        AddItem(ItemRegistry.CreateItem("pickaxe"), 1);
        AddItem(ItemRegistry.CreateItem("workbench"), 20);
    }
    
    public bool AddItem(Item newItem, int amount)
    {
        var existingSlot = Slots.FirstOrDefault(s => 
            s.Item != null &&
            s.Item.Tag == newItem.Tag && 
            s.Count < s.Item.MaxStack);

        if (existingSlot != null)
        {
            int canAdd = Math.Min(amount, existingSlot.Item.MaxStack - existingSlot.Count);
            existingSlot.Count += canAdd;
            amount -= canAdd;
        }

        while (amount > 0)
        {
            var emptySlot = Slots.FirstOrDefault(s => s.IsEmpty);
        
            if (emptySlot != null)
            {
                int toAdd = Math.Min(amount, newItem.MaxStack);
                emptySlot.Item = newItem;
                emptySlot.Count = toAdd;
                emptySlot.Image = newItem.Image;
                emptySlot.IsEmpty = false;
                amount -= toAdd;
            }
            else
                return false; 
        }
        return true;
    }

    public bool RemoveItem(string itemTag, int amount)
    {
        InventorySlot? slot;
        slot = Slots.FirstOrDefault(s => 
            s.Item != null &&
            s.Item.Tag == itemTag && 
            s.Count >= amount);
        if (slot != null)
        {
            slot.Count -= amount;

            if (slot.Count == 0)
            {
                slot.IsEmpty = true;
                slot.Image = null;
            }
            return true;
        }
        return false;
    }

    public bool EmptySlots()
    {
        var slot = Slots.FirstOrDefault(s => s.IsEmpty);
        if (slot != null) return true;
        return false;
    }
    
    public int GetItemCount(string itemTag)
    {
        int count = 0;
         var slot = Slots.FirstOrDefault(s => 
            s.Item != null &&
            s.Item.Tag == itemTag);
         if (slot != null) count = slot.Count;
        
        return count;
    }
    
}

