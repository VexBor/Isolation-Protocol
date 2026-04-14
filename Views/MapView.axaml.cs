using System;
using Avalonia.Controls;
using Avalonia.Input;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Views;

public partial class MapView : UserControl
{
    
    
    public MapView()
    {
        InitializeComponent();
        Focusable = true;
        AttachedToVisualTree += (s, e) => 
        {
            if (DataContext is MapViewModel vm)
            {
                vm.Renderer.Render(this.GameCanvas);
            }
        };
    }

    private void GameCanvas_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        var point = e.GetPosition(sender as Control);
    
        if (DataContext is MapViewModel vm)
        {
            vm.PlacementManager.UpdatePreview((int)point.X, (int)point.Y);
        }
    }

    private void GameCanvas_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetPosition(sender as Control);
    
        if (e.GetCurrentPoint(sender as Control).Properties.IsLeftButtonPressed)
        {
            if (DataContext is MapViewModel vm)
            {
                vm.PlacementManager.ExecutePlacement(point.X, point.Y);
            }
        }
    }
}