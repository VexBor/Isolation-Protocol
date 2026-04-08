using System;

namespace Isolation_Protocol.Models;

public readonly struct Vector2
{
    public double X { get; }
    public double Y { get; }

    public Vector2(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double Length => Math.Sqrt(X * X + Y * Y);
    
    // Нормалізація руху по діагоналі
    public Vector2 Normalize()
    {
        double len = Length;
        return len > 0 ? new Vector2(X / len, Y / len) : new Vector2(0, 0);
    }
    
    // Множення та додавання векторів
    public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator *(Vector2 a, double scalar) => new Vector2(a.X * scalar, a.Y * scalar);
}