namespace Isolation_Protocol.Models.Craft;

public class CraftIngredient(string itemId, int amount)
{
    public string ItemId { get; set; } = itemId;
    public int Amount { get; set; } = amount;
}