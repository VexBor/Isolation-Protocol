using System;
using Isolation_Protocol.Models;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Services;

public class CaveMap(int width, int height, Player player) : GameMap(width, height, player)
{
    private Random _rand = new();
    public double spawnX { get; set; }
    public double spawnY { get; set; }
    public void Generate()
    {
        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
        {
            Map[x, y] = new MapCell(CellType.CaveFloor);
            Map[x, y].X = x;
            Map[x, y].Y = y;
        }

        int totalCells = (width - 2) * (height - 2);
        int wallCount = (int)(totalCells * 0.30);

        int placed = 0;
        while (placed < wallCount)
        {
            int x = _rand.Next(1, width - 1);
            int y = _rand.Next(1, height - 1);

            if (Map[x, y].Type == CellType.CaveFloor)
            {
                Map[x, y].Type = CellType.CaveWall;
                placed++;
            }
        }
        
        int oreCount = (int)(totalCells * 0.05);

        placed = 0;
        while (placed < oreCount)
        {
            int x = _rand.Next(1, width - 1);
            int y = _rand.Next(1, height - 1);

            if (Map[x, y].Type == CellType.CaveWall)
            {
                Map[x,y].Type = CellType.CaveFloor;
                int oreType = _rand.Next(1, 4);
                switch (oreType)
                {
                    case 1:
                        Map[x,y].Object = new  IronOre();
                        break;
                    case 2:
                        Map[x,y].Object = new GoldOre();
                        break;
                    case 3:
                        Map[x,y].Object = new EmeraldOre();
                        break;
                }
                placed++;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(x == 0 || x == width - 1)
                    Map[x, y].Type = CellType.CaveWall;
                if(y == 0 || y == height - 1)
                    Map[x, y].Type = CellType.CaveWall;
            }
        }
        
        spawnX = GetRandomSafeSpawnPoint().X;
        spawnY = GetRandomSafeSpawnPoint().Y;
        
        Vector2 cavePos = GetRandomSafeSpawnPoint();
        Map[(int)cavePos.X / TileSize, (int)cavePos.Y / TileSize].Object = new Cave();
    }
}