using Avalonia;
using Isolation_Protocol.Models;

namespace Isolation_Protocol.Services;

public class PhysicsEngine
{
    private readonly double _playerSize = 30;
    private readonly double _speed = 200;    // Пікселів на секунду

    public void Update(Player player, GameMap map, Vector inputDirection, double deltaTime)
    {
        // Зміщення
        double moveX = inputDirection.X * _speed * deltaTime;
        double moveY = inputDirection.Y * _speed * deltaTime;

        // Перевірка
        if (!IsColliding(player.X + moveX, player.Y, map))
        {
            player.X += moveX;
        }
        
        if (!IsColliding(player.X, player.Y + moveY, map))
        {
            player.Y += moveY;
        }
    }

    private bool IsColliding(double newX, double newY, GameMap map)
    {
        double[] checkX = { newX, newX + _playerSize };
        double[] checkY = { newY, newY + _playerSize };

        foreach (var x in checkX)
        {
            foreach (var y in checkY)
            {
                // Перевод з координат канваса в Тайли
                int gridX = (int)(x / map.TileSize);
                int gridY = (int)(y / map.TileSize);

                var cell = map.GetCell(gridX, gridY);
                if (cell == null || !cell.IsWalkable) 
                    return true; // Пройти не можна
            }
        }
        return false;
    }
}