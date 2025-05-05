using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FCoreObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		MaterialInputNativeSerialize = 1,
		EnumProperties = 2,
		SkeletalMaterialEditorDataStripping = 3,
		FProperties = 4,
		VersionPlusOne = 5,
		LatestVersion = 4
	}

	public static readonly FGuid GUID = new FGuid(928956732u, 115624187u, 3036710128u, 640315774u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_22)
		{
			if (game >= EGame.GAME_UE4_12)
			{
				if (game < EGame.GAME_UE4_15)
				{
					return Type.MaterialInputNativeSerialize;
				}
				return Type.EnumProperties;
			}
			return Type.BeforeCustomVersionWasAdded;
		}
		if (game < EGame.GAME_UE4_25)
		{
			return Type.SkeletalMaterialEditorDataStripping;
		}
		return Type.FProperties;
	}
}
