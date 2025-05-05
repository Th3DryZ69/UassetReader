using CUE4Parse.UE4.Readers;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public class FFortCreativeVkPalette_ProjectInfo
{
	public int LinkVersion;

	public int unk;

	public FVkModuleVersionRef[] PublicModules;

	public FFortCreativeVkPalette_ProjectInfo(FArchive Ar)
	{
		LinkVersion = Ar.Read<int>();
	}
}
