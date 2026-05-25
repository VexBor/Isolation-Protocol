using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Isolation_Protocol.Interfaces;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.View;

public partial class MapViewModel : ViewModelBase
{
    [ObservableProperty]
    private Vector _scrollPos;
    
    [ObservableProperty]
    private PlacementManager _placementManager;
    
    public Player Player { get; set; } = new();
    public MapRenderer Renderer { get; set; }
    private double _viewWidth = 1920;
    private double _viewHeight = 1080;
    
    private InventoryViewModel _inventory = new(6);
    
    [ObservableProperty]
    private  InventoryViewModel _openInventory;
    
    [ObservableProperty]
    private CraftViewModel _craftOpened;
    
    [ObservableProperty]
    private EscapeMenuViewModel  _escapeMenu;
    
    [ObservableProperty]
    private InventoryViewModel? _currentChestInventory;
    
    [ObservableProperty]
    private RepairViewModel? _selectedRepair;
    
    [ObservableProperty]
    private AnvilViewModel _anvil;
    
    [ObservableProperty]
    private VictoryViewModel _victoryMenu;    
    
    [ObservableProperty]
    private DevPanelViewModel _devPanel;
    
    private User _user = Authorize.GetCurrentUser();
    private PhysicsEngine _physicsEngine = new PhysicsEngine();
    private GameMap _map { get; set; }
    private CaveMap _caveMap;
    private GameMap _activeMap;
    private CraftViewModel _craft;
    private Camera _camera = new Camera();
    private Stopwatch _stopwatch = new Stopwatch();
    private DropLogic _drop;
    private RepairViewModel _repair;

    private double _lastTickElapsed;
    private DispatcherTimer _timer;
    
    public bool IsUserAmin => _user.IsAdmin;
    
    public void Init()
    {
        _stopwatch.Start();
        _escapeMenu = new EscapeMenuViewModel(this);
        Renderer = new MapRenderer(_activeMap);
        _craft = new CraftViewModel(_inventory, _activeMap);
        _drop = new DropLogic(_inventory);
        _placementManager = new PlacementManager(_inventory, _activeMap, Renderer);
        OpenInventory = _inventory;
        _repair.StartRocket += WinGame;
        
        InputHandler.OnMouseClick += (mouseButton) => TryInteract(mouseButton);
        InputHandler.OnSelectSlot += (s) => ChanceSlot(s);

        _timer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        _timer.Tick += (s, e) => OnRender();
        _timer.Start();
    } 
    
    private void OnRender()
    {
        InputHandler.SelectedSlot();
        double currentTickElapsed = _stopwatch.Elapsed.TotalSeconds;
            
        double deltaTime = currentTickElapsed - _lastTickElapsed;
        _lastTickElapsed = currentTickElapsed;

        if (deltaTime > 0.1) deltaTime = 0.1;
        
        Vector2 vector = InputHandler.GetMovementDirection().Normalize();
        
        _physicsEngine.Update(Player, _activeMap, vector,deltaTime);
        ScrollPos = _camera.Update(Player.X, Player.Y, _viewWidth, _viewHeight, _activeMap.Width, _activeMap.Height, _activeMap.TileSize);
        
        if (_inventory.Slots[_inventory.SelectedSlot].Item != null &&  ObjectFactory.CreateWorldObject(_inventory.Slots[_inventory.SelectedSlot].Item.Tag) != null && _placementManager.IsPlacing == false && _activeMap == _map)
        {
            _placementManager.StartPlacement(ItemRegistry.CreateItem(_inventory.Slots[_inventory.SelectedSlot].Item.Tag));
        } 
        
        if (InputHandler.OpenCraftMenu())
        {
            if (CraftOpened == null)
            {
                CraftOpened = _craft;
                _stopwatch.Stop();
                OpenInventory = null;
            }
            else
            {
                _stopwatch.Start();
                OpenInventory = _inventory;
                CraftOpened = null;
            }
        }

        if (InputHandler.PressedQ() && InputHandler.IsShiftPressed())
        {
            _inventory.RemoveItemForSlot(_inventory.SelectedSlot);
        }

        if (InputHandler.Escape())
        {
            SaveGame();
            Logs.Save();
            Sound.PlaySfx("click");
            if (CraftOpened != null)
            {
                OpenInventory = _inventory;
                CraftOpened = null;
                Logs.Add($"player close craft menu");
                _stopwatch.Start();
                return;
            }

            if (_currentChestInventory != null)
            {
                _stopwatch.Start();
                Logs.Add($"player close chest");
                CloseChest();
                return;
            }

            if (Anvil != null)
            {
                _stopwatch.Start();
                Logs.Add($"player close anvil");
                Anvil = null;
                return;
            }
            
            if (SelectedRepair != null)
            {
                _stopwatch.Start();
                Logs.Add($"player close repair menu");
                SelectedRepair = null;
                return;
            }

            if (DevPanel != null)
            {
                Logs.Add($"player close dev menu");
                DevPanel = null;
                return;
            }
            
            _escapeMenu.TogglePause();
            
            if (_escapeMenu.IsPaused)
            {
                _stopwatch.Stop();
                Logs.Add($"player open escape menu");
                OpenInventory = null;
            }
            else
            {
                _stopwatch.Start();
                Logs.Add($"player close escape menu");
                OpenInventory = _inventory;
            }
        }
        
        if(Player.Stamina <= 0 && _physicsEngine.isWater)
        {
            Vector2 spawnPos = _map.GetRandomSafeSpawnPoint();
            Player.X = spawnPos.X;
            Player.Y = spawnPos.Y;
        }
    }

    public void NewGame()
    {       
        ItemRegistry.Initialize();
        Player = new();
        _inventory = new(6);
        _map = new GameMap(100,100, Player);
        _map.InitializeMap();
        
        _caveMap = new CaveMap(50, 50, Player);
        _caveMap.Generate();
        
        Vector2 spawnPos = _map.GetRandomSafeSpawnPoint();
        Player.X = spawnPos.X;
        Player.Y = spawnPos.Y;
        
        _repair = new  RepairViewModel(_inventory);
        _activeMap = _map;
        Init();
        
        Renderer.SwitchMap(_activeMap); 
        Renderer.Render();
        
        _inventory.AddItem(ItemRegistry.CreateItem("axe"), 1);
    }
    
    public void LoadGame()
    {
        ItemRegistry.Initialize();
        _map = Save.GetSaveMap();
        Player = Save.GetSavePlayer();
        _inventory = Save.GetSaveInventory();
        _inventory.InitImage();
        
        _repair = new  RepairViewModel(_inventory);
        _repair.Repaired = Save.GetModuleStatus();
        for(int i = 0; i < _repair.Repaired.Length; i++)
            _repair.Module[i].IsRepaired = _repair.Repaired[i];
        
        _caveMap = Save.GetSaveCaveMap();
        
        _inventory.SelectSlot(_inventory.Slots[0]);
        _inventory.SelectedSlot = 0;
        
        foreach (var cell in _map.Map)
        {
            if(cell.Object == null) continue;
            if (cell.Object.Tag == "chest")
            {
                cell.Object.ChestInventory.InitImage();
            }
        }
        
        _map.SetPlayer(Player);
        _caveMap.SetPlayer(Player);
        
        if(Player.isCurrentMapCave ) _activeMap = _caveMap;
        else _activeMap = _map;
        Init();
        
        Renderer.SwitchMap(_activeMap); 
        Renderer.Render();
    }

    public void SaveGame()
    {
        Save.SaveMap(_map);
        Save.SavePlayer(Player);
        Save.SaveCave(_caveMap);
        Save.SaveInventory(_inventory);
        Save.SaveModuleStatus(_repair.Repaired);
    }

    public void WinGame()
    {
        SelectedRepair = null;
        _stopwatch.Stop();
        _timer.Stop();
        VictoryMenu = new VictoryViewModel();
    }
    
    private void TryInteract(MouseButton mouseButton)
    {
        int gridX = (int)(Player.X / _activeMap.TileSize);
        int gridY = (int)(Player.Y / _activeMap.TileSize);

        MapCell? bestCell = null;
        float minDistance = float.MaxValue;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; 
                
                var cell = _activeMap.GetCell(gridX + x, gridY + y);
                if (cell?.Object == null) continue;

                float dist = Vector2.Distance(new Vector2(Player.X, Player.Y), new Vector2(cell.Object.X * 40, cell.Object.Y * 40));
            
                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestCell = cell;
                }
            }
        }

        if (mouseButton == MouseButton.Right &&  _inventory.Slots[_inventory.SelectedSlot].Item != null &&_inventory.Slots[_inventory.SelectedSlot].Item.Tag == "apple" && Player.Hunger < 100)
        {
            Player.Hunger += 10;
            _inventory.RemoveItem(_inventory.Slots[_inventory.SelectedSlot].Item.Tag, 1);
            Logs.Add("player eat apple");
            return;
        }
        
        if (bestCell != null)
        {
            if (mouseButton == MouseButton.Left) InteractWithCell(bestCell);
            else if (mouseButton == MouseButton.Right) InteractWithObject(bestCell);
        }
    }
    
    private async Task InteractWithCell(MapCell cell)
    {
        if (cell.Object != null)
        {
            Item tool;
            if(_inventory.Slots[_inventory.SelectedSlot].Item is Item item) tool = item;
            else return;
            
            if(Player.Stamina < 5) return;
            
            tool.Durability--;
            Player.Stamina -= 1;
            
            var interact = cell.Object;
            
            if (interact.OnInteract(tool))
            {
                if (tool.Durability <= 0)
                    _inventory.RemoveItem(item.Tag, 1);
                
                if (interact.VisualElement.Parent is Canvas parent)
                {
                    parent.Children.Remove(interact.VisualElement);
                }
                _drop.Drop(interact);
                
                Logs.Add($"player destroyed {interact.Tag}");
                
                cell.Object.VisualElement = null;
                cell.Object = null;
            }
        }
    }

    private void InteractWithObject(MapCell cell)
    {
        if (cell.Object !=  null)
        {
            if (cell.Object is Chest chest)
            {
                Sound.PlaySfx("chest");
                CurrentChestInventory = chest.ChestInventory;
                _inventory.TargetInventory = chest.ChestInventory;
                _stopwatch.Stop();
                chest.ChestInventory.TargetInventory = _inventory;
            }
            
            if (cell.Object is Workbench workbench)
            {
                Sound.PlaySfx("click");
                CraftOpened = _craft;
                _stopwatch.Stop();
                OpenInventory = null;
            }
            
            if (cell.Object is Rocket rocket)
            {
                SelectedRepair = _repair;
                _stopwatch.Stop();
                _repair.SetRocket(rocket);
                Sound.PlaySfx("click");
            }

            if (cell.Object is Cave cave)
            {
                ChanceMap();
                Sound.PlaySfx("click");
            }

            if (cell.Object is Anvil anvil)
            {
                _stopwatch.Stop();
                Anvil = new AnvilViewModel(_inventory);
                Sound.PlaySfx("click");
            }
            Logs.Add($"player open {cell.Object.Tag}");
        }
    }

    private void ChanceMap()
    {
        if (_activeMap == _map)
        {
            Renderer.SwitchMap(_caveMap);
            Player.oldPlayerPos = new Point(Player.X, Player.Y);
            Player.X = _caveMap.spawnX;
            Player.Y = _caveMap.spawnY;
            _activeMap = _caveMap;
            Renderer.Render();
            Logs.Add($"player go to cave");
            Player.isCurrentMapCave = true;
        }
        else
        {
            Renderer.SwitchMap(_map);
            _activeMap = _map;
            Player.X = Player.oldPlayerPos.X;
            Player.Y = Player.oldPlayerPos.Y;
            Player.isCurrentMapCave = false;
            Logs.Add($"player go to world");
            Renderer.Render();
        }
    }

    private void ChanceSlot(int? selectSlot)
    {
        if(selectSlot == null) return;
        if (selectSlot != _inventory.SelectedSlot)
        {
            Sound.PlaySfx("slot");
            _inventory.SelectedSlot = (int)selectSlot;

            foreach (InventorySlot slot in _inventory.Slots)
            {
                slot.IsSelected = false;
            }

            _inventory.Slots[_inventory.SelectedSlot].IsSelected = true;
            Logs.Add($"player select slot number {_inventory.SelectedSlot}");
            _placementManager.StopPlacement();
        }
    }
    
    [RelayCommand]
    public void CloseChest()
    {
        _stopwatch.Start();
        Sound.PlaySfx("click");
        Logs.Add($"player close chest");
        CurrentChestInventory!.TargetInventory = null;
        CurrentChestInventory = null;
        _inventory.TargetInventory = null;
    }

    [RelayCommand]
    public void OpenDevPanel()
    {
        DevPanel = new(_inventory, Player);
        Logs.Add($"player open dev panel");
        Sound.PlaySfx("click");
        
    }

    [RelayCommand]
    public void CloseDevPanel()
    {
        DevPanel = null;
        Logs.Add($"player close dev panel");
        Sound.PlaySfx("click");
        
    }
}