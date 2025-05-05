using System;
using CUE4Parse.UE4.Assets.Exports.Material.Parameters;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructFallback]
public class FStaticParameterSet
{
	public FStaticSwitchParameter[] StaticSwitchParameters;

	public FStaticComponentMaskParameter[] StaticComponentMaskParameters;

	public FStaticTerrainLayerWeightParameter[] TerrainLayerWeightParameters;

	public FStaticMaterialLayersParameter[] MaterialLayersParameters;

	public FStaticParameterSet(FArchive Ar)
	{
		StaticSwitchParameters = Ar.ReadArray(() => new FStaticSwitchParameter(Ar));
		StaticComponentMaskParameters = Ar.ReadArray(() => new FStaticComponentMaskParameter(Ar));
		TerrainLayerWeightParameters = Ar.ReadArray(() => new FStaticTerrainLayerWeightParameter(Ar));
		if (FReleaseObjectVersion.Get(Ar) >= FReleaseObjectVersion.Type.MaterialLayersParameterSerializationRefactor)
		{
			MaterialLayersParameters = Ar.ReadArray(() => new FStaticMaterialLayersParameter(Ar));
		}
	}

	public FStaticParameterSet(FStructFallback fallback)
	{
		StaticSwitchParameters = fallback.GetOrDefault("StaticSwitchParameters", Array.Empty<FStaticSwitchParameter>());
		StaticComponentMaskParameters = fallback.GetOrDefault("StaticComponentMaskParameters", Array.Empty<FStaticComponentMaskParameter>());
		TerrainLayerWeightParameters = fallback.GetOrDefault("TerrainLayerWeightParameters", Array.Empty<FStaticTerrainLayerWeightParameter>());
		MaterialLayersParameters = fallback.GetOrDefault("MaterialLayersParameters", Array.Empty<FStaticMaterialLayersParameter>());
	}
}
