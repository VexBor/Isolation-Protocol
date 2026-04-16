using System;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Isolation_Protocol.Models;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Services;

public partial class PlacementManager(InventoryViewModel inventory, GameMap map, MapRenderer mapRenderer) : ObservableObject
{
    private InventoryViewModel _inventory =  inventory;
    private GameMap _map =  map;
    private MapRenderer _mapRenderer = mapRenderer;
    
    [ObservableProperty]
    private IImage? _previewIcon = null;

    [ObservableProperty] private int _previewX;
    [ObservableProperty] private int _previewY;
    [ObservableProperty] private bool _isPlacing = false;
    [ObservableProperty] private double _opacity = 0.5;
    
    private Item? _currentItemToPlace;

    public void StartPlacement(Item item)
    {
        _currentItemToPlace = item;
        IsPlacing = true;
        
        PreviewIcon = _currentItemToPlace.Image;
    }
    
    public void ExecutePlacement(double mouseX, double mouseY)
    {
        var slotItemTag = _inventory.Slots[_inventory.SelectedSlot].Item.Tag;
        
        if(_currentItemToPlace == null) return;
        
        if (_currentItemToPlace.Tag != slotItemTag)
        {
            _currentItemToPlace = ItemRegistry.CreateItem(slotItemTag);
            PreviewIcon = _currentItemToPlace!.Image;
        }
        
        if (!IsPlacing || _currentItemToPlace == null) return;
        
        int gx = (int)(mouseX / _map.TileSize); 
        int gy = (int)(mouseY / _map.TileSize);

        if (!_map.CanPlaceAt(gx, gy)) return;

        int count = _inventory.GetItemCount(_currentItemToPlace.Tag);
        if (count <= 0)
        {
            StopPlacement();
            return;
        }

        Place(gx, gy);
    }

    private void Place(int x, int y)
    {
        if(ObjectFactory.CreateWorldObject(_currentItemToPlace.Tag) == null) return;
        MapObject mapObject = ObjectFactory.CreateWorldObject(_currentItemToPlace.Tag);
        mapObject.X = x;
        mapObject.Y = y;
        
        _mapRenderer.DrawObject(mapObject);
        _map.AddObject(mapObject, x,y);
        
        _inventory.RemoveItem(_currentItemToPlace.Tag, 1);

        if (_inventory.GetItemCount(_currentItemToPlace.Tag) <= 0) StopPlacement();
    }

    public void StopPlacement()
    {
        IsPlacing = false;
        PreviewIcon = null;
        _currentItemToPlace = null;
    }

    public void UpdatePreview(int x, int y)
    {
        int tileX = (x / _map.TileSize);
        int tileY = (y / _map.TileSize);
        
        PreviewX = x - (int)(0.5 * _map.TileSize);
        PreviewY = y - (int)(0.5 * _map.TileSize);

        if (_map.CanPlaceAt(tileX, tileY)) Opacity = 0.5;
        else Opacity = 0;
    }
}