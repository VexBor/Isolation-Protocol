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
            ImageId = "axe",
            Durability = 100,
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/axe.png")
        };
        
        _items["axe_stone"] = new Item 
        {
            Tag = "axe_stone",
            ItemType = ItemType.Tool,
            Damage = 15,
            ImageId = "axeStone",
            Durability = 150,
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/axeStone.png")
        };
        
        _items["axe_iron"] = new Item 
        {
            Tag = "axe_iron",
            ItemType = ItemType.Tool,
            Damage = 20,
            ImageId = "axeIron",
            Durability = 200,
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/axeIron.png")
        };
    
        _items["pickaxe"] = new Item
        {
            Tag = "pickaxe",
            ItemType = ItemType.Tool,
            Damage = 10,
            Durability = 100,
            ImageId = "pickaxe",
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/pickaxe.png")
        };
        
        _items["pickaxe_stone"] = new Item
        {
            Tag = "pickaxe_stone",
            ItemType = ItemType.Tool,
            Damage = 20,
            Durability = 150,
            ImageId = "pickaxeStone",
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/pickaxeStone.png")
        };
        
        _items["pickaxe_iron"] = new Item
        {
            Tag = "pickaxe_iron",
            ItemType = ItemType.Tool,
            Damage = 25,
            Durability = 250,
            ImageId = "pickaxeIron",
            MaxStack = 1,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/pickaxeIron.png")
        };
    
        _items["stone"] = new Item
        {
            Tag = "stone",
            ItemType = ItemType.Resource,
            ImageId = "StoneItem",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/StoneItem.png")
        };
    
        _items["wood"] = new Item
        {
            Tag = "wood",
            ItemType = ItemType.Resource,
            ImageId = "wood",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/wood.png")
        };
    
        _items["workbench"] = new Item
        {
            Tag = "workbench",
            ItemType = ItemType.Resource,
            ImageId = "workbench",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/workbench.png")
        };
    
        _items["chest"] = new Item
        {
            Tag = "chest",
            ItemType = ItemType.Resource,
            ImageId = "chest",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/chest.png")
        };

        _items["iron"] = new Item
        {
            Tag = "iron",
            ItemType = ItemType.Resource,
            ImageId = "ironIngot",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/ironIngot.png")
        };
        
        _items["gold"] = new Item
        {
            Tag = "gold",
            ItemType = ItemType.Resource,
            ImageId = "goldIngot",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/goldIngot.png")
        };
        
        _items["emerald"] = new Item
        {
            Tag = "emerald",
            ItemType = ItemType.Resource,
            ImageId = "emeraldIngot",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/emeraldIngot.png")
        };
        _items["anvil"] = new Item
        {
            Tag = "anvil",
            ItemType = ItemType.Resource,
            ImageId = "anvil",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/anvil.png")
        };
        _items["iron_plate"] = new Item
        {
            Tag = "iron_plate",
            ItemType = ItemType.Resource,
            ImageId = "ironPlate",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/ironPlate.png")
        };
        _items["gold_plate"] = new Item
        {
            Tag = "gold_plate",
            ItemType = ItemType.Resource,
            ImageId = "goldPlate",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/goldPlate.png")
        };
        _items["motor"] = new Item
        {
            Tag = "motor",
            ItemType = ItemType.Resource,
            ImageId = "motor",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/motor.png")
        };
        _items["apple"] = new Item
        {
            Tag = "apple",
            ItemType = ItemType.Resource,
            ImageId = "apple",
            MaxStack = 32,
            Image = LoadBitmap("avares://Isolation Protocol/Assets/apple.png")
        };
    }

    public static Item? CreateItem(string tag) 
    {
        return _items.TryGetValue(tag, out var item) ? item.Clone() : null;
    }

    private static Bitmap LoadBitmap(string uri) 
        => new Bitmap(AssetLoader.Open(new Uri(uri)));
}