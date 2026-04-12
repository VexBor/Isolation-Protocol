using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace Isolation_Protocol.Models;

public partial class Player : ObservableObject
{
    [ObservableProperty]
    private double _x = 2000;
    
    [ObservableProperty]
    private double _y = 2000;
    
    [ObservableProperty] 
    public double _health = 100;

    [ObservableProperty]
    private double _stamina = 100;
    
    [ObservableProperty]
    private double _hunger = 100;
    
    public int Height { get; set; } = 20;
    public int Width { get; set; } = 20;
    
    public int Speed { get; set; } = 200;
    public int Armor { get; set; }
    public int Strength { get; set; }
}