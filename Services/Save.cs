using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using Isolation_Protocol.Models;
using Isolation_Protocol.View;
using Newtonsoft.Json;

namespace Isolation_Protocol.Services;

public static class Save
{
    private static string _currentSaveName = "Default/";
    private static string path = "Assets/Data/World/";

    public static bool Exist => Directory.Exists(path + _currentSaveName);
    
    public static void SaveMap(GameMap map)
    {
        if (!Directory.Exists(path + _currentSaveName))
            Directory.CreateDirectory(path + _currentSaveName);

        var data = JsonConvert.SerializeObject(map);
        File.WriteAllText(path + _currentSaveName + "map.json", data);
    }
    
    public static void SaveCave(CaveMap cave)
    {
        if (!Directory.Exists(path + _currentSaveName))
            Directory.CreateDirectory(path + _currentSaveName);

        var data = JsonConvert.SerializeObject(cave);
        File.WriteAllText(path + _currentSaveName + "cave.json", data);
    }
    
    public static void SavePlayer(Player player)
    {
        if (!Directory.Exists(path + _currentSaveName))
            Directory.CreateDirectory(path + _currentSaveName);
        
        var data = JsonConvert.SerializeObject(player);
        File.WriteAllText(path + _currentSaveName + "player.json", data);

    }

    public static void SaveInventory(InventoryViewModel inventory)
    {
        if (!Directory.Exists(path + _currentSaveName))
            Directory.CreateDirectory(path + _currentSaveName);
        
        var data = JsonConvert.SerializeObject(inventory);
        File.WriteAllText(path + _currentSaveName + "inventory.json", data);
    }

    public static void SaveModuleStatus(bool[] moduleStatus)
    {
        if (!Directory.Exists(path + _currentSaveName))
            Directory.CreateDirectory(path + _currentSaveName);
        
        var data = JsonConvert.SerializeObject(moduleStatus);
        File.WriteAllText(path + _currentSaveName + "modulestatus.json", data);
    }

    public static bool[] GetModuleStatus()
    {
        var data = File.ReadAllText(path + _currentSaveName + "modulestatus.json");
        return JsonConvert.DeserializeObject<bool[]>(data);
    }
    
    public static InventoryViewModel GetSaveInventory()
    {
        var data = File.ReadAllText(path + _currentSaveName + "inventory.json");
        return JsonConvert.DeserializeObject<InventoryViewModel>(data);
    }
    public static GameMap GetSaveMap()
    {
        GameMap map;
        var data = File.ReadAllText(path + _currentSaveName + "map.json");
        map = JsonConvert.DeserializeObject<GameMap>(data);

        foreach (var cell in map.Map)
        {
            if(cell.Object == null) continue;
            switch (cell.Object.Tag)
            {
                case "tree":
                    cell.Object = new Tree()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y,
                        Health = cell.Object.Health
                    };
                    break;
                case "stone":
                    cell.Object = new Stone()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y,
                        Health = cell.Object.Health
                    };
                    break;
                case "chest":
                    cell.Object = new Chest()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y,
                        Health = cell.Object.Health,
                        ChestInventory = cell.Object.ChestInventory
                    };
                    break;
                case "workbench":
                    cell.Object = new Workbench()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y,
                        Health = cell.Object.Health
                    };
                    break;
                case "cave":
                    cell.Object = new Cave()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y
                    };
                    break;
                case "rocket":
                    cell.Object = new Rocket()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y
                    };
                    if(GetModuleStatus().All(m => m)) cell.Object.TextureId = "roket";
                    break;
                case "anvil":
                    cell.Object = new Anvil()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y
                    };
                    break;
            }
        }
        
        return map;
    }
    
    public static CaveMap GetSaveCaveMap()
    {
        var data = File.ReadAllText(path + _currentSaveName + "cave.json");
        var map = JsonConvert.DeserializeObject<CaveMap>(data);
        foreach (var cell in map.Map)
        {
            if(cell.Object == null) continue;
            switch (cell.Object.Tag)
            {
                case "ironOre":
                    cell.Object = new IronOre()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y,
                        Health = cell.Object.Health
                    };
                    break;
                case "goldOre":
                    cell.Object = new GoldOre()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y,
                        Health = cell.Object.Health
                    };
                    break;
                case "emeraldOre":
                    cell.Object = new EmeraldOre()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y,
                        Health = cell.Object.Health
                    };
                    break;
                case "cave":
                    cell.Object = new Cave()
                    {
                        X = cell.Object.X,
                        Y = cell.Object.Y
                    };
                    break;
            }
        }
        return map;
    }

    public static Player GetSavePlayer()
    {
        var data = File.ReadAllText(path + _currentSaveName + "player.json");
        return JsonConvert.DeserializeObject<Player>(data);
    }
}