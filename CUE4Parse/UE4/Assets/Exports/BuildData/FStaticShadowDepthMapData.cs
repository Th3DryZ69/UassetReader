using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FStaticShadowDepthMapData
{
	public FMatrix WorldToLight;

	public int ShadowMapSizeX;

	public int ShadowMapSizeY;

	public FFloat16[]? DepthSamples;

	public FStaticShadowDepthMapData(FArchive Ar)
	{
		WorldToLight = new FMatrix(Ar);
		ShadowMapSizeX = Ar.Read<int>();
		ShadowMapSizeY = Ar.Read<int>();
		DepthSamples = Ar.ReadArray(() => new FFloat16(Ar));
	}
}
