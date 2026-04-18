using System.Collections.Generic;
using System.Text.Json.Serialization;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models.Craft;

public class CraftRecipe(string resultItemTag, List<CraftIngredient> ingredients, bool needWorkBench, int amount, int craftTime)
{
    public string ResultItemTag { get; set; } = resultItemTag;
    public string DisplayName => Localization.Instance[$"Item_{ResultItemTag}_Name"];
    public string DisplayDescription => Localization.Instance[$"Item_{ResultItemTag}_Desc"];
    public List<CraftIngredient> Ingredients { get; set; } = ingredients;
    public bool NeedWorkBench { get; set; } = needWorkBench;
    public int Amount { get; set; } = amount;
    public int CraftTime { get; set; } = craftTime;
}