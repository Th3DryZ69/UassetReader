using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FMobileObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		InstancedStaticMeshLightmapSerialization = 1,
		LQVolumetricLightmapLayers = 2,
		StoreReflectionCaptureCompressedMobile = 3,
		VersionPlusOne = 4,
		LatestVersion = 3
	}

	public static readonly FGuid GUID = new FGuid(2955626933u, 3139454185u, 2734961335u, 1390674784u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game >= EGame.GAME_UE4_19)
		{
			if (game < EGame.GAME_UE4_26)
			{
				return Type.LQVolumetricLightmapLayers;
			}
			return Type.StoreReflectionCaptureCompressedMobile;
		}
		return Type.BeforeCustomVersionWasAdded;
	}
}
