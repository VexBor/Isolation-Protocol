using System;
using Avalonia;

namespace Isolation_Protocol.Services;

public class Camera
{
    public double Smoothing { get; set; } = 0.05;

    private double _currentX;
    private double _currentY;

    public Vector Update(double targetX, double targetY, double viewWidth, double viewHeight, int mapWidth, int mapHeight, int tileSize)
    {
        double goalX = targetX - (viewWidth / 2);
        double goalY = targetY - (viewHeight / 2);

        double maxScrollX = (mapWidth * tileSize) - viewWidth;
        double maxScrollY = (mapHeight * tileSize) - viewHeight;

        goalX = Math.Clamp(goalX, 0, maxScrollX);
        goalY = Math.Clamp(goalY, 0, maxScrollY);

        _currentX += (goalX - _currentX) * Smoothing;
        _currentY += (goalY - _currentY) * Smoothing;

        return new Vector(_currentX, _currentY);
    }
}