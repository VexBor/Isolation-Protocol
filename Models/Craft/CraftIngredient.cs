namespace Isolation_Protocol.Models.Craft;

public class CraftIngredient(string itemTag, int amount)
{
    public string ItemTag { get; set; } = itemTag;
    public int Amount { get; set; } = amount;
}