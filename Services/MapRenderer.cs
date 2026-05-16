using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.Services;

public class MapRenderer
{
    private Canvas _canvas;
    private GameMap map;
    private Border _player;
    private Image _placement;
    
    public MapRenderer(GameMap m)
    {
        map = m;
    }

    public void Init(Canvas canvas)
    {
        _canvas = canvas;
    }
    
    public void Render()
    {
        if (_canvas == null) return;

        foreach (var child in _canvas.Children)
        {
            if (child is Border bord && bord.Name == "Player") _player =  bord;
            if (child is Image img && img.Name == "Placement") _placement = img;
        }
        
        _canvas.Children.Clear();
        _canvas.Children.Add(_player);
        _canvas.Children.Add(_placement);
        
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                var cell = map.Map[x, y];
                
                var rect = new Border
                {
                    Width = map.TileSize,
                    Height = map.TileSize,
                    Classes = { "MapCell", cell.Type.ToString() },
                    IsHitTestVisible = false,
                    Focusable =  false
                };

                Canvas.SetLeft(rect, x * map.TileSize);
                Canvas.SetTop(rect, y * map.TileSize);
                _canvas.Children.Add(rect);
            }
        }
        
        var objectsToDraw = GetObjectsOrderedByY();
    
        foreach (MapObject obj in objectsToDraw) {
            if(obj.Tag == "chest") DrawObject(obj);
            else if(obj.Tag == "workbench") DrawObject(obj);
            else if(obj.Tag == "emeraldOre") DrawObject(obj);
            else if(obj.Tag == "ironOre") DrawObject(obj);
            else if(obj.Tag == "goldOre") DrawObject(obj);
            else DrawOre(obj);
        }
    }
    public void DrawOre(MapObject obj)
    {
        var image = new Image { Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Isolation Protocol/Assets/{obj.TextureId}.png"))), Width = map.TileSize * 1.5, Height = map.TileSize * 2, ZIndex = 10};
        
        Canvas.SetLeft(image, obj.X * map.TileSize - map.TileSize * 0.25);
        if(obj is Tree) Canvas.SetTop(image, obj.Y * map.TileSize - map.TileSize);
        else Canvas.SetTop(image, obj.Y * map.TileSize - 0.5 * map.TileSize);
        
        obj.VisualElement = image;
        _canvas.Children.Add(image);
    }
    
    public void DrawObject(MapObject obj)
    {
        var image = new Image { Source = new Bitmap(AssetLoader.Open(new Uri($"avares://Isolation Protocol/Assets/{obj.TextureId}.png"))), Width = map.TileSize, Height = map.TileSize, ZIndex = 8};
        
        Canvas.SetLeft(image, obj.X * map.TileSize);
        Canvas.SetTop(image, obj.Y * map.TileSize);

        obj.VisualElement = image;
        _canvas.Children.Add(image);
    }
    
    private List<MapObject> GetObjectsOrderedByY()
    {
        var allObjects = new List<MapObject>();

        foreach (MapCell cell in map.Map)
        {
            if (cell.Object != null)
            {
                cell.Object.X = cell.X;
                cell.Object.Y = cell.Y;
                allObjects.Add(cell.Object);
            }
        }

        return new List<MapObject>(allObjects.OrderBy(obj => obj.Y));
    }
    
    public void SwitchMap(GameMap newMap)
    {
        map = newMap;
    }
}