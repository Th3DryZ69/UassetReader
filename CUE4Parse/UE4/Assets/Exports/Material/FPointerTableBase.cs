using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FPointerTableBase
{
	public FTypeLayoutDesc[] TypeDependencies;

	public virtual void LoadFromArchive(FArchive Ar, bool bUseNewFormat)
	{
		TypeDependencies = Ar.ReadArray(() => new FTypeLayoutDesc(Ar, bUseNewFormat));
	}
}
