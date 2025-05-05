using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.BuildData;

[JsonConverter(typeof(FMeshMapBuildDataConverter))]
public class FMeshMapBuildData
{
	public FLightMap? LightMap;

	public FShadowMap? ShadowMap;

	public FGuid[] IrrelevantLights;

	public FPerInstanceLightmapData[] PerInstanceLightmapData;

	public FMeshMapBuildData(FAssetArchive Ar)
	{
		switch (Ar.Read<ELightMapType>())
		{
		case ELightMapType.LMT_None:
			LightMap = null;
			break;
		case ELightMapType.LMT_1D:
			LightMap = new FLegacyLightMap1D(Ar);
			break;
		case ELightMapType.LMT_2D:
			LightMap = new FLightMap2D(Ar);
			break;
		}
		switch (Ar.Read<EShadowMapType>())
		{
		case EShadowMapType.SMT_None:
			ShadowMap = null;
			break;
		case EShadowMapType.SMT_2D:
			ShadowMap = new FShadowMap2D(Ar);
			break;
		}
		IrrelevantLights = Ar.ReadArray<FGuid>();
		PerInstanceLightmapData = Ar.ReadBulkArray<FPerInstanceLightmapData>();
	}
}
