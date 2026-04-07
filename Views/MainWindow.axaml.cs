using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

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
}