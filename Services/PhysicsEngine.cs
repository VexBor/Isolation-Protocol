using Isolation_Protocol.Models;

namespace Isolation_Protocol.Services;

public class PhysicsEngine
{

    public void Update(Player player, GameMap map, Vector2 inputDirection, double deltaTime)
    {
        // Зміщення
        double moveX = inputDirection.X * player.Speed * deltaTime;
        double moveY = inputDirection.Y * player.Speed * deltaTime;

        // Перевірка
        if (!IsColliding(player.X + moveX, player.Y, map, player.Height, player.Width))
        {
            player.X += moveX;
        }
        
        if (!IsColliding(player.X, player.Y + moveY, map, player.Height, player.Width))
        {
            player.Y += moveY;
        }
    }

    private bool IsColliding(double newX, double newY, GameMap map, int playerHeight, int playerWidth)
    {
        double[] checkX = { newX, newX + playerWidth };
        double[] checkY = { newY, newY + playerHeight };

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