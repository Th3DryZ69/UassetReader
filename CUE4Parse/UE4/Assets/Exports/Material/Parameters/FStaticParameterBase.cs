using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material.Parameters;

[StructFallback]
public class FStaticParameterBase
{
	public FMaterialParameterInfo? ParameterInfo;

	public bool bOverride;

	public FGuid ExpressionGuid;

	[JsonIgnore]
	public string Name => ParameterInfo?.Name.Text ?? "None";

	public FStaticParameterBase()
	{
	}

	public FStaticParameterBase(FStructFallback fallback)
	{
		ParameterInfo = fallback.GetOrDefault<FMaterialParameterInfo>("ParameterInfo");
		bOverride = fallback.GetOrDefault("bOverride", defaultValue: false);
		ExpressionGuid = fallback.GetOrDefault<FGuid>("ExpressionGuid");
	}

	public FStaticParameterBase(FArchive Ar)
	{
		ParameterInfo = ((FRenderingObjectVersion.Get(Ar) < FRenderingObjectVersion.Type.MaterialAttributeLayerParameters) ? new FMaterialParameterInfo
		{
			Name = Ar.ReadFName()
		} : new FMaterialParameterInfo(Ar));
	}
}
