using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Isolation_Protocol.Models;

public class GameMap(int width, int height, Player player)
{
    public int TileSize { get; } = 40;
    public int Width { get; } = width;
    public int Height { get; } = height;
    public MapCell[,] Map { get; } = new MapCell[width, height];

    private Player _player = player;

    public void InitializeMap()
    {
        Random rand = new Random();
        double seed = rand.NextDouble() * 1000;
        double scale = 0.07; // Масштаб шуму (чим менше, тим більші острови)

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
                else if (finalHeight < 0.7)
                {
                    double temp = rand.NextDouble();
                    Map[x, y].Type = CellType.Floor; // Трава/земля
                    Map[x, y].X = x;
                    Map[x, y].Y = y;
                    
                    if (temp < 0.1) Map[x, y].Object = new Tree();
                    else if (temp < 0.12) Map[x, y].Object = new Stone();
                }
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
    
    public Vector2 GetRandomSafeSpawnPoint()
    {
        Random rand = new Random();
        int safeX = 0;
        int safeY = 0;
        bool found = false;

        while (!found)
        {
            int x = rand.Next(0, Width);
            int y = rand.Next(0, Height);

            if (Map[x, y].Type == CellType.Floor && Map[x, y].Object == null)
            {
                safeX = x * TileSize;
                safeY = y * TileSize;
                found = true;
            }
        }

        return new Vector2(safeX, safeY);
    }

    public bool CanPlaceAt(int x, int y)
    {
        if (GetCell(x, y).Object != null) return false;
        if((int)(player.X / TileSize) ==  x && (int) (player.Y / TileSize) == y) return false;
        if((int)(player.X / TileSize) >=  x + 3 || (int) (player.Y / TileSize) >= y + 3) return false;
        if((int)(player.X / TileSize) <=  x - 3 || (int) (player.Y / TileSize) <= y - 3) return false;
        return true;
    }

    public bool ObjectInVision(string objectTag)
    {
        int gridX = (int)_player.X / TileSize;
        int gridY = (int)_player.Y / TileSize;
        
        List<MapCell> cells = new List<MapCell>();
        
        cells.Add(Map[gridX + 1, gridY]);
        cells.Add(Map[gridX - 1, gridY]);
        cells.Add(Map[gridX, gridY + 1]);
        cells.Add(Map[gridX, gridY - 1]);
        
        cells.Add(Map[gridX + 1, gridY + 1]);
        cells.Add(Map[gridX - 1, gridY + 1]);
        cells.Add(Map[gridX - 1, gridY - 1]);
        cells.Add(Map[gridX + 1, gridY - 1]);
        
        if(cells.FirstOrDefault(o => o.Object != null && o.Object.Tag == objectTag) != null) return true;
        
        return false;
    }
    
    public void AddObject(MapObject obj,  int x, int y)
    {
        Map[x, y].Object = obj;
    }
}