using Avalonia.Controls;
using Isolation_Protocol.View;

namespace Isolation_Protocol.Views;

public partial class MapView : UserControl
{
    public MapView()
    {
        InitializeComponent();
        AttachedToVisualTree += (s, e) => 
        {
            if (DataContext is MapViewModel vm)
            {
                vm.Renderer.Render(this.GameCanvas);
            }
        };
    }
}