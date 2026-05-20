using Avalonia;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models;

public class EmeraldOre : MapObject
{
    public EmeraldOre()
    {
        IsPassable = false;
        Tag = "emeraldOre";
        Health = 200f;
        TextureId = "emeraldOre";
        MaxHealth = 200f;
        Drop =
        [
            new ResourceDrop(ItemRegistry.CreateItem("emerald"), 2, 5)
        ];
    }
    
    public override bool OnInteract(Item? tool)
    {
        if (tool.Tag == "pickaxe_iron") 
        {
            Sound.PlaySfx("stone");
            Health -= tool.Damage;
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40), 40, (Health / MaxHealth));
            if(Health <= 0) return true;
        }
        return false;
    }
}