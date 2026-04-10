using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Views;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
    }

    public void Exit_Click(object? sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
    
    
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        InputHandler.RegisterKeyDown(e.Key);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);
        InputHandler.RegisterKeyUp(e.Key);
    }
}