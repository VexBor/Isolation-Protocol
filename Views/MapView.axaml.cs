using System;
using Avalonia.Controls;
using Avalonia.Input;
using Isolation_Protocol.Services;
using Isolation_Protocol.View;
using MouseButton = Isolation_Protocol.Services.MouseButton;

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
                UIHelper.Init(this.GameCanvas);
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
                InputHandler.SafeInvoke(MouseButton.Left);
            }
        }
        
        if (e.GetCurrentPoint(sender as Control).Properties.IsRightButtonPressed)
        {
            if (DataContext is MapViewModel vm)
            {
                InputHandler.SafeInvoke(MouseButton.Right);
            }
        }
    }
}