using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

public class FLightComponentMapBuildData
{
	public int ShadowMapChannel;

	public FStaticShadowDepthMapData DepthMap;

	public FLightComponentMapBuildData(FArchive Ar)
	{
		ShadowMapChannel = Ar.Read<int>();
		DepthMap = new FStaticShadowDepthMapData(Ar);
	}
}
