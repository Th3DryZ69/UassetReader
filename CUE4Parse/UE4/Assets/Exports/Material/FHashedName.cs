using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public struct FHashedName
{
	public ulong Hash;

	public FHashedName(FArchive Ar)
	{
		Hash = Ar.Read<ulong>();
	}
}
