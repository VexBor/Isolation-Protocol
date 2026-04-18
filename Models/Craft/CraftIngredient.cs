using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models.Craft;

public class CraftIngredient(string itemTag, int amount)
{
    public string ItemTag { get; set; } = itemTag;
    public string DisplayName => Localization.Instance[$"Item_{ItemTag}_Name"];
    public int Amount { get; set; } = amount;
}