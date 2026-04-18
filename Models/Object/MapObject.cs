using Avalonia.Controls;
using Avalonia.Media;
using Isolation_Protocol.Interfaces;

namespace Isolation_Protocol.Models;

public class MapObject : IInteractable
{
    private IInteractable _interactableImplementation;
    public string? Tag { get; set; }
    public int Health { get; set; }
    public bool IsPassable { get; set; }
    public IImage? Image { get; set; }
    
    public Image? VisualElement { get; set; }
    public ResourceDrop? Drop { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    
    public bool OnInteract(Item? tool)
    {
        return _interactableImplementation.OnInteract(tool);
    }
}