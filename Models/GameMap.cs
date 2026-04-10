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
        /*Random rand = new Random();
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
        Map[2, 1].Type = CellType.Floor; */
        Random rand = new Random();
        double seed = rand.NextDouble() * 1000;
        double scale = 0.1; // Масштаб шуму (чим менше, тим більші острови)

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Map[x, y] = new MapCell(CellType.Floor);
                // 1. Отримуємо значення шуму Перліна (від 0 до 1)
                double noiseValue = Math.Abs(Perlin2D(x * scale + seed, y * scale + seed));

                // 2. Накладаємо маску острова (відстань від центру)
                double centerX = width / 2.0;
                double centerY = height / 2.0;
                double dist = Math.Sqrt(Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2));
                double maxDist = Math.Min(width, height) / 2.2; // Радіус суходолу

                // "Притискаємо" шум до країв
                double mask = Math.Clamp((maxDist - dist) / maxDist, 0, 1);
                double finalHeight = noiseValue * mask;

                // 3. Перетворюємо висоту в типи тайлів
                if (finalHeight < 0.1) Map[x, y].Type = CellType.Water;      // Глибока вода
                else if (finalHeight < 0.2) Map[x, y].Type = CellType.Sand;   // Пляж
                else if (finalHeight < 0.7) Map[x, y].Type = CellType.Floor;  // Трава/земля
                else Map[x, y].Type = CellType.Wall;                         // Скелі/гори
            }
        }
    }
    
    public MapCell GetCell(int x, int y)
    {
        if (x < 0 || x > Width || y < 0 || y > Height) return null;
        return Map[x, y];
    }
    
    private double Perlin2D(double x, double y)
    {
        return (Math.Sin(x) + Math.Cos(y) + Math.Sin(x + y)) / 3; 
    }
    
}