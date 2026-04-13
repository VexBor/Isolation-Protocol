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

    private static int _slot = 0;
    
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

    public static bool IsSprint()
    {
        return IsKeyDown(Key.LeftShift ) || IsKeyDown(Key.RightShift);
    }

    public static bool IsInteract()
    {
        return IsKeyDown(Key.Space);
    }

    public static int SelectedSlot()
    {
        if (IsKeyDown(Key.D1)) _slot = 0;
        if (IsKeyDown(Key.D2)) _slot = 1;
        if (IsKeyDown(Key.D3)) _slot = 2;
        if (IsKeyDown(Key.D4)) _slot = 3;
        if (IsKeyDown(Key.D5)) _slot = 4;
        if (IsKeyDown(Key.D6)) _slot = 5;
        
        return _slot;
    }

    public static bool OpenCraftMenu()
    {
        if (IsKeyDown(Key.R))
        {
            RegisterKeyUp(Key.R);
            return true;
        }
        return false;
    }
}