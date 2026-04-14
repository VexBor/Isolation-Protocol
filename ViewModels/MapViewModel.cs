using System;
using System.Diagnostics;
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
    
    public static Player Player { get; set; } = new();
    public MapRenderer Renderer { get; set; }
    private double _viewWidth = 1920;
    private double _viewHeight = 1080;
    
    [ObservableProperty]
    private static InventoryViewModel _inventory = new(Player);
    
    [ObservableProperty]
    private CraftViewModel _craftOpened;

    private PhysicsEngine _physicsEngine = new PhysicsEngine();
    private GameMap _map { get; set; }
    private CraftViewModel _craft;
    private Camera _camera = new Camera();
    private Stopwatch _stopwatch = new Stopwatch();
    private DropLogic _drop;
    private double _lastTickElapsed;
    private bool _isReloadet = true;
    private double _reloadTime = 1;

    public MapViewModel()
    {
        ItemRegistry.Initialize();
        _stopwatch.Start();
        _map = new GameMap(100,100, Player);
        _map.InitializeMap();
        Renderer = new MapRenderer(_map);
        _craft = new CraftViewModel(_inventory);
        _drop = new DropLogic(_inventory);
        _placementManager = new PlacementManager(_inventory, _map, Renderer);

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
        double currentTickElapsed = _stopwatch.Elapsed.TotalSeconds;
            
        double deltaTime = currentTickElapsed - _lastTickElapsed;
        _lastTickElapsed = currentTickElapsed;

        if (deltaTime > 0.1) deltaTime = 0.1;
        
        Vector2 vector = InputHandler.GetMovementDirection().Normalize();
        
        _physicsEngine.Update(Player, _map, vector,deltaTime);
        ScrollPos = _camera.Update(Player.X, Player.Y, _viewWidth, _viewHeight, _map.Width, _map.Height, _map.TileSize);
        
        if (InputHandler.IsInteract())
        {
            int gridX = (int)(Player.X / _map.TileSize);
            int gridY = (int)(Player.Y / _map.TileSize);
            
            MapCell cell = null;
            
            if (_map.GetCell(gridX + 1, gridY).Object != null) cell = _map.GetCell(gridX + 1, gridY);
            else if (_map.GetCell(gridX - 1, gridY).Object != null) cell =_map.GetCell(gridX - 1, gridY);
            else if (_map.GetCell(gridX, gridY + 1).Object != null) cell =_map.GetCell(gridX, gridY + 1);
            else if (_map.GetCell(gridX, gridY - 1).Object != null) cell =_map.GetCell(gridX, gridY - 1);
            if(cell == null) return;
            
            InteractWithCell(cell);
        }

        if (!_isReloadet)
        {
            _reloadTime -= deltaTime;
            if (_reloadTime <= 0)
            {
                _isReloadet = true;
                _reloadTime = 1;
            }
        }

        if (InputHandler.SelectedSlot() != _inventory.SelectedSlot)
        {
            _inventory.SelectedSlot = InputHandler.SelectedSlot();

            foreach (InventorySlot slot in _inventory.Slots)
            {
                slot.IsSelected = false;
            }

            InventorySlot selectedSlot = _inventory.Slots[_inventory.SelectedSlot];
            selectedSlot.IsSelected = true;
            if (selectedSlot.Item != null &&  selectedSlot.Item.Object != null)
            {
                _placementManager.StartPlacement(ItemRegistry.CreateItem(selectedSlot.Item.Tag));
                return;
            } 
            _placementManager.StopPlacement();
        }

        if (InputHandler.OpenCraftMenu())
        {
            if (CraftOpened == null) CraftOpened = _craft;
            else CraftOpened = null;
        }
    }
    
    private void InteractWithCell(MapCell cell)
    {
        if (cell.Object is IInteractable interactable)
        {
            Item tool;
            if(_inventory.Slots[_inventory.SelectedSlot].Item is Item item) tool = item;
            else return;
            
            if (interactable.OnInteract(tool) && _isReloadet)
            {
                tool.Durability--;
                
                if (cell.Object.VisualElement.Parent is Canvas parent)
                {
                    parent.Children.Remove(cell.Object.VisualElement);
                }
                
                cell.Object.VisualElement = null;
                cell.Object = null;
                _drop.Drop(interactable);
                _isReloadet = false;
            }
        }
    }
}