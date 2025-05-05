using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FDestructionObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		AddedTimestampedGeometryComponentCache = 1,
		AddedCacheDataReduction = 2,
		GeometryCollectionInDDC = 3,
		GeometryCollectionInDDCAndAsset = 4,
		ChaosArchiveAdded = 5,
		FieldsAdded = 6,
		DensityUnitsChanged = 7,
		BulkSerializeArrays = 8,
		GroupAndAttributeNameRemapping = 9,
		ImplicitObjectDoCollideAttribute = 10,
		VersionPlusOne = 11,
		LatestVersion = 10
	}

	public static readonly FGuid GUID = new FGuid(391061259u, 3032892837u, 2973708008u, 3506147709u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_23)
		{
			if (game < EGame.GAME_UE4_22)
			{
				return Type.BeforeCustomVersionWasAdded;
			}
			return Type.AddedCacheDataReduction;
		}
		if (game < EGame.GAME_UE4_25)
		{
			return Type.GroupAndAttributeNameRemapping;
		}
		return Type.ImplicitObjectDoCollideAttribute;
	}
}
