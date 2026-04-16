using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Isolation_Protocol.Models.Craft;

public class CraftRecipe(string resultItemTag, string resultItemName,List<CraftIngredient> ingredients, bool needWorkBench, int amount, int craftTime, string? description)
{
    public string ResultItemTag { get; set; } = resultItemTag;
    public string ResultItemName { get; set; } = resultItemName;
    public List<CraftIngredient> Ingredients { get; set; } = ingredients;
    public bool NeedWorkBench { get; set; } = needWorkBench;
    public string? Description { get; set; } = description;
    public int Amount { get; set; } = amount;
    public int CraftTime { get; set; } = craftTime;
}