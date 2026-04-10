using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Isolation_Protocol.Models;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.ViewModels;

public partial class MapViewModel : ViewModelBase
{
    public Player Player { get; set; } = new();
    public GameMap Map { get; set; } = new(100,100);
    public MapRenderer Renderer { get; set; }
    private double _viewWidth = 1920;
    private double _viewHeight = 1080;
    
    [ObservableProperty]
    private Vector _scrollPos;
    
    private PhysicsEngine _physicsEngine = new PhysicsEngine();
    private Camera _camera = new Camera();
    private Stopwatch _stopwatch = new Stopwatch();
    private double _lastTickElapsed;

    public MapViewModel()
    {
        _stopwatch.Start();
        Map.InitializeMap();
        Renderer = new MapRenderer(Map);
        
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
        
        _physicsEngine.Update(Player, Map, vector,deltaTime);
        ScrollPos = _camera.Update(Player.X, Player.Y, _viewWidth, _viewHeight, Map.Width, Map.Height, Map.TileSize);
    }
}