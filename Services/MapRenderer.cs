using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.Services;

public class MapRenderer(GameMap map)
{
    public void Render(Canvas? canvas)
    {
        if (canvas == null) return;
        

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
                canvas.Children.Add(rect);
            }
        }
        
        var objectsToDraw = GetObjectsOrderedByY();
    
        foreach (MapObject obj in objectsToDraw) {
            if (obj is Tree) DrawTree(canvas, obj);
        }
    }
    private void DrawTree(Canvas canvas, MapObject treeObj)
    {
        var image = new Image { Source = treeObj.Image, Width = map.TileSize * 1.5, Height = map.TileSize * 2, ZIndex = 10};
        Canvas.SetLeft(image, treeObj.X * map.TileSize - map.TileSize * 0.25); // Центруємо
        Canvas.SetTop(image, treeObj.Y * map.TileSize - map.TileSize);      // Зміщуємо крону вгору
        
        canvas.Children.Add(image);
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
}