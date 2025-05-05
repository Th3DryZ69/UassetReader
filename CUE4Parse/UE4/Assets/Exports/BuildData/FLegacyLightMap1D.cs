using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FLegacyLightMap1D : FLightMap
{
	public FLegacyLightMap1D(FAssetArchive Ar)
		: base(Ar)
	{
		throw new ParserException("Unsupported: FLegacyLightMap1D");
	}
}
