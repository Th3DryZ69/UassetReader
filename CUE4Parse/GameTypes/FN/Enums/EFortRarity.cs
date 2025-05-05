using System.ComponentModel;

namespace CUE4Parse.GameTypes.FN.Enums;

public enum EFortRarity : byte
{
	[Description("Uncommon")]
	Uncommon = 1,
	[Description("Unattainable")]
	Impossible = 7,
	[Description("Unattainable")]
	Unattainable = 7,
	[Description("Exotic")]
	Exotic = 6,
	[Description("Exotic")]
	Transcendent = 6,
	[Description("Mythic")]
	Elegant = 5,
	[Description("Mythic")]
	Mythic = 5,
	[Description("Legendary")]
	Fine = 4,
	[Description("Legendary")]
	Legendary = 4,
	[Description("Epic")]
	Quality = 3,
	[Description("Epic")]
	Epic = 3,
	[Description("Rare")]
	Sturdy = 2,
	[Description("Rare")]
	Rare = 2,
	[Description("Common")]
	Handmade = 0,
	[Description("Common")]
	Common = 0
}
