using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace Isolation_Protocol.Models;

public static class ItemRegistry
{
    private static readonly Dictionary<string, Item> _items = new();

    public static void Initialize()
    {
        _items["axe"] = new Item 
        {
            Tag = "axe",
            ItemType = ItemType.Tool,
            Damage = 10,
            Durability = 100,
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/axe.png")
        };
    
        _items["pickaxe"] = new Item
        {
            Tag = "pickaxe",
            ItemType = ItemType.Tool,
            Damage = 10,
            Durability = 100,
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/pickaxe.png")
        };
    
        _items["stone"] = new Item
        {
            Tag = "stone",
            ItemType = ItemType.Resource,
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/StoneItem.png")
        };
    
        _items["wood"] = new Item
        {
            Tag = "wood",
            ItemType = ItemType.Resource,
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/wood.png")
        };
    
        _items["workbench"] = new Item
        {
            Tag = "workbench",
            ItemType = ItemType.Resource,
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/workbench.png")
        };
    
        _items["chest"] = new Item
        {
            Tag = "chest",
            ItemType = ItemType.Resource,
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/chest.png")
        };
    }

    public static Item? CreateItem(string tag) 
    {
        return _items.TryGetValue(tag, out var item) ? item.Clone() : null;
    }

    private static Bitmap LoadBitmap(string uri) 
        => new Bitmap(AssetLoader.Open(new Uri(uri)));
}