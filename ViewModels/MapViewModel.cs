using System;
using System.Diagnostics;
using Avalonia.Threading;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.ViewModels;

public class MapViewModel : ViewModelBase
{
    public Player Player { get; set; } = new();
    public GameMap Map { get; set; } = new(500,500);

    private PhysicsEngine _physicsEngine = new PhysicsEngine();
    private Stopwatch _stopwatch = new Stopwatch();
    private double _lastTickElapsed;

    public MapViewModel()
    {
        _stopwatch.Start();
        Map.InitializeMap();
        
        var timer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(1)
        };
        timer.Tick += (s, e) => OnRender();
        timer.Start();
    }
    
    private void OnRender()
    {
        double currentTickElapsed = _stopwatch.Elapsed.TotalSeconds;
            
        double deltaTime = currentTickElapsed - _lastTickElapsed;
        _lastTickElapsed = currentTickElapsed;

        if (deltaTime > 0.1) deltaTime = 0.1;
        
        Vector2 vector = InputHandler.GetMovementDirection().Normalize();
        
        _physicsEngine.Update(Player, Map, vector,deltaTime);
    }
}