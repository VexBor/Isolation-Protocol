using System;

namespace Isolation_Protocol.Models;

public class GameMap(int width, int height)
{
    public int TileSize { get; } = 40;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public MapCell[,] Map { get; } = new MapCell[width, height];

    public void InitializeMap()
    {
        Random rand = new Random();
        double wallChance = 0.2; // 20% шансу, що клітинка буде стіною

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    Map[x, y] = new MapCell(CellType.Wall);
                }
                else
                {
                    if (rand.NextDouble() < wallChance)
                        Map[x, y] = new MapCell(CellType.Wall);
                    else
                        Map[x, y] = new MapCell(CellType.Floor);
                }
            }
        }
        
        Map[1, 1].Type = CellType.Floor;
        Map[1, 2].Type = CellType.Floor;
        Map[2, 1].Type = CellType.Floor;
    }
    
    public MapCell GetCell(int x, int y)
    {
        if (x < 0 || x > Width || y < 0 || y > Height) return null;
        return Map[x, y];
    }
    
}