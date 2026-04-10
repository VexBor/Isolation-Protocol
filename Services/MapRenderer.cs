using Avalonia.Controls;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.Services;

public class MapRenderer
{
    private readonly GameMap _map;

    public MapRenderer(GameMap map)
    {
        _map = map;
    }

    public void Render(Canvas canvas)
    {
        if (canvas == null) return;
        

        for (int x = 0; x < _map.Width; x++)
        {
            for (int y = 0; y < _map.Height; y++)
            {
                var cell = _map.Map[x, y];
                var rect = new Border
                {
                    Width = _map.TileSize,
                    Height = _map.TileSize,
                    Classes = { "MapCell", cell.Type.ToString() },
                    IsHitTestVisible = false,
                    Focusable =  false
                };

                Canvas.SetLeft(rect, x * _map.TileSize);
                Canvas.SetTop(rect, y * _map.TileSize);
                canvas.Children.Add(rect);
            }
        }
    }
}