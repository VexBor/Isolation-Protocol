namespace Isolation_Protocol.Models;

public class ResourceDrop(Item dropItem,int minAmount, int maxAmount)
{
    public Item DropItem { get; set; } = dropItem;
    public int MinAmount { get; set; } =  minAmount;
    public int MaxAmount { get; set; } =   maxAmount;
}