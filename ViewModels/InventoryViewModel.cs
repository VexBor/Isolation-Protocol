using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class InventoryViewModel: ViewModelBase
{
    [ObservableProperty]
    private InventoryViewModel? _targetInventory;
    
    public List<InventorySlot> Slots { get; set; } = new List<InventorySlot>();
    public int SelectedSlot { get; set; }
    
    public InventoryViewModel(int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            Slots.Add(new InventorySlot());
        }
        
        Slots[0].IsSelected = true;
    }
    
    public bool AddItem(Item newItem, int amount)
    {
        if(newItem == null) return false;
        
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
                slot.Item = null;
                slot.IsEmpty = true;
                slot.Image = null;
            }
            return true;
        }
        return false;
    }

    public bool EmptySlots(string tag)
    {
        var slot = Slots.FirstOrDefault(s => s.Item != null && s.Item.Tag == tag && s.Count < s.Item.MaxStack || s.IsEmpty);
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
    
    [RelayCommand]
    public void SelectSlot(InventorySlot clickedSlot)
    {
        if (InputHandler.IsShiftPressed())
        {
            TransferItem(clickedSlot);
            return;
        }
        if (clickedSlot == null) return;

        foreach (var slot in Slots)
        {
            slot.IsSelected = false;
        }

        clickedSlot.IsSelected = true;
    
        SelectedSlot = Slots.IndexOf(clickedSlot);
    
        System.Diagnostics.Debug.WriteLine($"Вибрано слот: {SelectedSlot}, Предмет: {clickedSlot.Item?.Tag ?? "Пусто"}");
    }
    
    public void TransferItem(InventorySlot slot)
    {
        if (TargetInventory == null || slot.IsEmpty || slot.Item == null) return;

        bool success = TargetInventory.AddItem(slot.Item, slot.Count);
        
        if (success)
        {
            RemoveItem(slot.Item.Tag, slot.Count);
        }
    }
}

