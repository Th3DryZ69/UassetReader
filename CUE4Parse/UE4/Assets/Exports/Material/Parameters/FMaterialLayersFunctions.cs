using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material.Parameters;

public class FMaterialLayersFunctions
{
	public string KeyString;

	public FMaterialLayersFunctions(FArchive Ar)
	{
		KeyString = Ar.ReadFString();
	}
}
