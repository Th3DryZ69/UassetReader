using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FTypeLayoutDesc
{
	public readonly FName? Name;

	public readonly FHashedName? NameHash;

	public readonly uint SavedLayoutSize;

	public readonly FSHAHash SavedLayoutHash;

	public FTypeLayoutDesc(FArchive Ar, bool bUseNewFormat)
	{
		if (bUseNewFormat)
		{
			Name = Ar.ReadFName();
		}
		else
		{
			NameHash = Ar.Read<FHashedName>();
		}
		SavedLayoutSize = Ar.Read<uint>();
		SavedLayoutHash = new FSHAHash(Ar);
	}
}
