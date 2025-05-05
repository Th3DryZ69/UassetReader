using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FNiagaraObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		SkeletalMeshVertexSampling = 1,
		VersionPlusOne = 2,
		LatestVersion = 1
	}

	public static readonly FGuid GUID = new FGuid(4071542956u, 2600354159u, 2254744191u, 4196849404u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		if (Ar.Game < EGame.GAME_UE4_21)
		{
			return Type.BeforeCustomVersionWasAdded;
		}
		return Type.SkeletalMeshVertexSampling;
	}
}
