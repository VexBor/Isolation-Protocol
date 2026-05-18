using System.Collections.Generic;
using Isolation_Protocol.Models.Craft;
using Isolation_Protocol.Services;

namespace Isolation_Protocol.Models;

public class RepairModule(string nameKey, List<CraftIngredient> ingredients)
{
    public string NameKey { get; } = nameKey;
    public bool IsRepaired { get; set; } = false;
    public List<CraftIngredient> Ingredients { get; } = ingredients;
    
    public override string ToString() => Localization.Instance[NameKey];
}