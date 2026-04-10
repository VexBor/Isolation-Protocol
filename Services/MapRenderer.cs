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
    }
}