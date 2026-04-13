using Isolation_Protocol.Models;

namespace Isolation_Protocol.Services;

public class PhysicsEngine
{
    private bool _isWater = false;

    public void Update(Player player, GameMap map, Vector2 inputDirection, double deltaTime)
    {
        // Зміщення
        double moveX = inputDirection.X * player.Speed * deltaTime;
        double moveY = inputDirection.Y * player.Speed * deltaTime;

        if (_isWater && player.Stamina > 0)
        {
            moveX *= 0.5;
            moveY *= 0.5;
            player.Stamina -= 0.2;
        }
        
        if (InputHandler.IsSprint() && (moveY != 0 || moveX != 0) && player.Speed > 0)
        {
            moveX *= 1.3;
            moveY *= 1.3;
            player.Stamina -= 0.2;
        }
        else if(player.Stamina < 100 && player.Hunger > 20 && !InputHandler.IsSprint() && !_isWater)
        {
            player.Stamina += 0.2;
            player.Hunger -= 0.05;
        }

        if (player.Stamina <= 0)
        {
            moveX = 0;
            moveY = 0;
        }
        
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
                if(cell.Object != null &&  !cell.Object.IsPassable)
                    return true;
                if(cell.Type == CellType.Water) _isWater = true;
                else _isWater = false;
            }
        }
        return false;
    }
}