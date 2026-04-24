using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Isolation_Protocol.Services;

public class UIHelper
{
    private static Canvas _canvas;

    public static void Init(Canvas canvas)
    {
        _canvas = canvas;
    }
    
    public async static void DrawProgressBar(Point position, double width, float progress)
    {
        var background = new Border
        {
            Width = width,
            Height = 6,
            Background = Brushes.Black,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(2),
            ZIndex = 20
        };

        var healthBar = new Border
        {
            Width = width * Math.Clamp(progress, 0, 1),
            Height = 4,
            Background = Brushes.Red,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
            ZIndex = 20
        };

        background.Child = healthBar;

        Canvas.SetLeft(background, position.X);
        Canvas.SetTop(background, position.Y);

        _canvas.Children.Add(background);
        
        await Task.Delay(1000);
        _canvas.Children.Remove(background);
    }
}