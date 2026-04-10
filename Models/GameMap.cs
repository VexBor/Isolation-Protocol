namespace Isolation_Protocol.Models;

public class GameMap(int width, int height)
{
    public int TileSize { get; } = 40;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public MapCell[,] Map { get; } = new MapCell[width, height];

    public void InitializeMap()
    {
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            Map[x, y] = new MapCell(CellType.Floor);
    }
    
    public MapCell GetCell(int x, int y)
    {
        if (x < 0 || x > Width || y < 0 || y > Height) return null;
        return Map[x, y];
    }
}