namespace Isolation_Protocol.Models.Craft;

public class CraftIngredient(string itemTag,string itemName, int amount)
{
    public string ItemTag { get; set; } = itemTag;
    public string ItemName { get; set; } = itemName;
    public int Amount { get; set; } = amount;
}