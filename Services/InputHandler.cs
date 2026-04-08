using System.Collections.Generic;
using System.Numerics;
using Avalonia.Input;
using Vector2 = Isolation_Protocol.Models.Vector2;

namespace Isolation_Protocol.Services;

public static class InputHandler
{
    private static readonly HashSet<Key> _keys = new();
    
    public static void RegisterKeyDown(Key key) => _keys.Add(key);
    public static void RegisterKeyUp(Key key) => _keys.Remove(key);

    private static bool IsKeyDown(Key key) => _keys.Contains(key);

    public static Vector2 GetMovementDirection()
    {
        double x = 0;
        double y = 0;

        if (IsKeyDown(Key.W)) y -= 1;
        if (IsKeyDown(Key.S)) y += 1;
        if (IsKeyDown(Key.A)) x -= 1;
        if (IsKeyDown(Key.D)) x += 1;

        return new Vector2(x, y);
    }
}