using ReactiveUI;

namespace Isolation_Protocol.Models;

public class Player : ReactiveObject
{
    private double _x = 50;
    private double _y = 50;

    public double X
    {
        get => _x;
        set => this.RaiseAndSetIfChanged(ref _x, value); 
    }

    public double Y
    {
        get => _y;
        set => this.RaiseAndSetIfChanged(ref _y, value);
    }
    public int Height { get; set; } = 32;
    public int Width { get; set; } = 32;
    
    public int Speed { get; set; } = 200;
    public int Health { get; set; }
    public int Stamina { get; set; }
    public int Armor { get; set; }
    public int Strength { get; set; }
}