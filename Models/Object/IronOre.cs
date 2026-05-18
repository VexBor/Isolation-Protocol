using Avalonia;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models;

public class IronOre : MapObject
{
    public IronOre()
    {
        IsPassable = false;
        Tag = "ironOre";
        Health = 100f;
        TextureId = "ironOre";
        MaxHealth = 100f;
        Drop = new ResourceDrop(ItemRegistry.CreateItem("iron"), 2, 5);
    }
    
    public override bool OnInteract(Item? tool)
    {
        if (tool.Tag == "pickaxe_stone" ||  tool.Tag == "pickaxe_iron") 
        {
            Sound.PlaySfx("stone");
            Health -= tool.Damage;
            UIHelper.DrawProgressBar(new Point(X * 40, Y * 40), 40, (Health / MaxHealth));
            if(Health <= 0) return true;
        }
        return false;
    }
}