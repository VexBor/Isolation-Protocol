using CommunityToolkit.Mvvm.ComponentModel;

namespace Isolation_Protocol.ViewModels;

public partial class PlayerViewModel : ViewModelBase
{
    // Позиція
    [ObservableProperty]
    private double _x = 100;

    [ObservableProperty]
    private double _y = 100;

    public double Width { get; } = 32;
    public double Height { get; } = 32;
    public double Speed { get; } = 200;
}