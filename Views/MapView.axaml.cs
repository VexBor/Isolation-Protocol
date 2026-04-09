using System;
using System.Runtime.Intrinsics.X86;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Isolation_Protocol.Services;
using Isolation_Protocol.ViewModels;

namespace Isolation_Protocol.Views;

public partial class MapView : UserControl
{
    private DispatcherTimer _gameTimer;
    private DateTime _lastTick = DateTime.Now;
    
    public MapView()
    {
        InitializeComponent();
        
        Focusable = true;
        AttachedToVisualTree += (s, e) => Focus();
        
        KeyDown += (s, e) => InputHandler.RegisterKeyDown(e.Key);
        KeyUp += (s, e) => InputHandler.RegisterKeyUp(e.Key);
        
        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16)
        };
        
        _gameTimer.Tick += OnTick;
        
        _lastTick = DateTime.Now;
        _gameTimer.Start();
    }
    
    private void OnTick(object? sender, EventArgs e)
    {
        var now = DateTime.Now;
        double deltaTime = (now - _lastTick).TotalSeconds;
        _lastTick = now;

        deltaTime = Math.Min(deltaTime, 0.1);

        UpdateGame(deltaTime);
    }
    
    private void UpdateGame(double dt)
    {
        var direction = InputHandler.GetMovementDirection().Normalize();
        
        if (DataContext is PlayerViewModel vm && direction.Length > 0)
        {
            vm.X += direction.X * vm.Speed * dt;
            vm.Y += direction.Y * vm.Speed * dt;
            
            if(vm.X < 0) vm.X = 0;
            if (vm.X > GameCanvas.Width - vm.Width) vm.X = GameCanvas.Width - vm.Width;
            if (vm.Y < 0) vm.Y = 0;
            if( vm.Y > GameCanvas.Height - vm.Height) vm.Y = GameCanvas.Height - vm.Height;
        }
    }
}