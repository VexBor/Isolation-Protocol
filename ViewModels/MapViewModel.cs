using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.ViewModels;

public partial class MapViewModel : ViewModelBase
{
    [ObservableProperty]
    private Vector _scrollPos;
    
    public Player Player { get; set; } = new();
    public MapRenderer Renderer { get; set; }
    private double _viewWidth = 1920;
    private double _viewHeight = 1080;

    private PhysicsEngine _physicsEngine = new PhysicsEngine();
    private GameMap _map { get; set; } = new(100,100);
    private Camera _camera = new Camera();
    private Stopwatch _stopwatch = new Stopwatch();
    private double _lastTickElapsed;

    public MapViewModel()
    {
        _stopwatch.Start();
        _map.InitializeMap();
        Renderer = new MapRenderer(_map);
        
        var timer = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(16)
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
        
        _physicsEngine.Update(Player, _map, vector,deltaTime);
        ScrollPos = _camera.Update(Player.X, Player.Y, _viewWidth, _viewHeight, _map.Width, _map.Height, _map.TileSize);
    }
}