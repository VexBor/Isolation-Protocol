using System;
using System.Diagnostics;
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
    private EscapeMenuViewModel  _escapeMenu = new();
    
    [ObservableProperty]
    private InventoryViewModel? _currentChestInventory;

    private PhysicsEngine _physicsEngine = new PhysicsEngine();
    private GameMap _map { get; set; }
    private CraftViewModel _craft;
    private Camera _camera = new Camera();
    private Stopwatch _stopwatch = new Stopwatch();
    private DropLogic _drop;
    private double _lastTickElapsed;

    public MapViewModel()
    {
        ItemRegistry.Initialize();
        _stopwatch.Start();
        _map = new GameMap(100,100, Player);
        _map.InitializeMap();
        Renderer = new MapRenderer(_map);
        _craft = new CraftViewModel(_inventory, _map);
        _drop = new DropLogic(_inventory);
        _placementManager = new PlacementManager(_inventory, _map, Renderer);
        OpenInventory = _inventory;
        
        InputHandler.OnMouseClick += (mouseButton) => TryInteract(mouseButton);
        InputHandler.OnSelectSlot += (s) => ChanceSlot(s);
        
        _inventory.AddItem(ItemRegistry.CreateItem("axe"), 1);
        _inventory.AddItem(ItemRegistry.CreateItem("pickaxe"), 1);
        _inventory.AddItem(ItemRegistry.CreateItem("workbench"), 20);
        _inventory.AddItem(ItemRegistry.CreateItem("chest"), 20);
        
        Vector2 spawnPos = _map.GetRandomSafeSpawnPoint();
        Player.X = spawnPos.X;
        Player.Y = spawnPos.Y;
        
        var timer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        timer.Tick += (s, e) => OnRender();
        timer.Start();

    }
    
    private void OnRender()
    {
        InputHandler.SelectedSlot();
        double currentTickElapsed = _stopwatch.Elapsed.TotalSeconds;
            
        double deltaTime = currentTickElapsed - _lastTickElapsed;
        _lastTickElapsed = currentTickElapsed;

        if (deltaTime > 0.1) deltaTime = 0.1;
        
        Vector2 vector = InputHandler.GetMovementDirection().Normalize();
        
        _physicsEngine.Update(Player, _map, vector,deltaTime);
        ScrollPos = _camera.Update(Player.X, Player.Y, _viewWidth, _viewHeight, _map.Width, _map.Height, _map.TileSize);
        
        if (_inventory.Slots[_inventory.SelectedSlot].Item != null &&  ObjectFactory.CreateWorldObject(_inventory.Slots[_inventory.SelectedSlot].Item.Tag) != null && _placementManager.IsPlacing == false)
        {
            _placementManager.StartPlacement(ItemRegistry.CreateItem(_inventory.Slots[_inventory.SelectedSlot].Item.Tag));
        } 
        
        if (InputHandler.OpenCraftMenu())
        {
            if (CraftOpened == null)
            {
                CraftOpened = _craft;
                OpenInventory = null;
            }
            else
            {
                OpenInventory = _inventory;
                CraftOpened = null; }
        }

        if (InputHandler.Escape())
        {
            Sound.PlaySfx("click");
            if (CraftOpened != null)
            {
                OpenInventory = _inventory;
                CraftOpened = null;
                return;
            }
            
            _escapeMenu.TogglePause();
            
            if (_escapeMenu.IsPaused)
            {
                _stopwatch.Stop();
                OpenInventory = null;
            }
            else
            {
                _stopwatch.Start();
                OpenInventory = _inventory;
            }
        }
    }

    private void TryInteract(MouseButton mouseButton)
    {
        int gridX = (int)(Player.X / _map.TileSize);
        int gridY = (int)(Player.Y / _map.TileSize);

        MapCell? bestCell = null;
        float minDistance = float.MaxValue;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue; 
                
                var cell = _map.GetCell(gridX + x, gridY + y);
                if (cell?.Object == null) continue;

                float dist = Vector2.Distance(new Vector2(Player.X, Player.Y), new Vector2(cell.Object.X, cell.Object.Y));
            
                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestCell = cell;
                }
            }
        }

        if (bestCell != null)
        {
            if (mouseButton == MouseButton.Left) InteractWithCell(bestCell);
            else if (mouseButton == MouseButton.Right) InteractWithObject(bestCell);
        }
    }
    
    private async Task InteractWithCell(MapCell cell)
    {
        if (cell.Object is IInteractable interactable)
        {
            Item tool;
            if(_inventory.Slots[_inventory.SelectedSlot].Item is Item item) tool = item;
            else return;
            
            tool.Durability--;
            
            if (interactable.OnInteract(tool))
            {
                if (tool.Durability <= 0)
                {
                    _inventory.RemoveItem(item.Tag, 1);
                    return;
                }
                
                if (cell.Object.VisualElement.Parent is Canvas parent)
                {
                    parent.Children.Remove(cell.Object.VisualElement);
                }
                
                cell.Object.VisualElement = null;
                cell.Object = null;
                _drop.Drop(interactable);
            }
        }
    }

    private void InteractWithObject(MapCell cell)
    {
        if (cell.Object is IInteractable interactable)
        {
            if (cell.Object is Chest chest)
            {
                Sound.PlaySfx("chest");
                CurrentChestInventory = chest.ChestInventory;
                _inventory.TargetInventory = chest.ChestInventory;
                chest.ChestInventory.TargetInventory = _inventory;
            }
            
            if (cell.Object is Workbench workbench)
            {
                Sound.PlaySfx("click");
                CraftOpened = _craft;
                OpenInventory = null;
            }
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
            _placementManager.StopPlacement();
        }
    }
    
    [RelayCommand]
    public void CloseChest()
    {
        Sound.PlaySfx("click");
        CurrentChestInventory!.TargetInventory = null;
        CurrentChestInventory = null;
        _inventory.TargetInventory = null;
    }
}