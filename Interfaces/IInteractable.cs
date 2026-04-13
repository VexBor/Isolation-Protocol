using Isolation_Protocol.Models;

namespace Isolation_Protocol.Interfaces;

public interface IInteractable
{
    bool OnInteract(Item? tool);
}