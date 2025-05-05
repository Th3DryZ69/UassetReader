using CUE4Parse.UE4.Objects.Core.i18N;

namespace CUE4Parse.GameTypes.FN.Enums;

public static class RarityUtil
{
	private static readonly FText _unattainable = new FText("Fort.Rarity", "Unattainable", "Unattainable");

	private static readonly FText _transcendent = new FText("Fort.Rarity", "Transcendent", "Transcendent");

	private static readonly FText _mythic = new FText("Fort.Rarity", "Mythic", "Mythic");

	private static readonly FText _legendary = new FText("Fort.Rarity", "Legendary", "Legendary");

	private static readonly FText _epic = new FText("Fort.Rarity", "Epic", "Epic");

	private static readonly FText _rare = new FText("Fort.Rarity", "Rare", "Rare");

	private static readonly FText _uncommon = new FText("Fort.Rarity", "Uncommon", "Uncommon");

	private static readonly FText _common = new FText("Fort.Rarity", "Common", "Common");

	public static FText GetNameText(this EFortRarity rarity)
	{
		return rarity switch
		{
			EFortRarity.Uncommon => _uncommon, 
			EFortRarity.Impossible => _unattainable, 
			EFortRarity.Exotic => _transcendent, 
			EFortRarity.Elegant => _mythic, 
			EFortRarity.Fine => _legendary, 
			EFortRarity.Quality => _epic, 
			EFortRarity.Sturdy => _rare, 
			EFortRarity.Handmade => _common, 
			_ => _uncommon, 
		};
	}
}
