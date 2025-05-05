using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderMapPointerTable : FPointerTableBase
{
	public FHashedName[]? Types;

	public FHashedName[]? VFTypes;

	public override void LoadFromArchive(FArchive Ar, bool bUseNewFormat)
	{
		if (bUseNewFormat)
		{
			base.LoadFromArchive(Ar, bUseNewFormat);
		}
		int length = Ar.Read<int>();
		int length2 = Ar.Read<int>();
		Types = Ar.ReadArray<FHashedName>(length);
		VFTypes = Ar.ReadArray<FHashedName>(length2);
		if (!bUseNewFormat)
		{
			base.LoadFromArchive(Ar, bUseNewFormat);
		}
	}
}
