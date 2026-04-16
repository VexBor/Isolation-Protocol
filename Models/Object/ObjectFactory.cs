namespace Isolation_Protocol.Models;

public static class ObjectFactory
{
    public static MapObject? CreateWorldObject(string tag)
    {
        return tag switch
        {
            "chest" => new Chest(),
            "workbench" => new Workbench(),
            _ => null
        };
    }
}