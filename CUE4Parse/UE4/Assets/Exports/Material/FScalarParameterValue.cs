using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructFallback]
public class FScalarParameterValue : IUStruct
{
	public readonly FName ParameterName;

	public readonly float ParameterValue;

	public readonly FMaterialParameterInfo ParameterInfo;

	public readonly FGuid ExpressionGUID;

	[JsonIgnore]
	public string Name => ((!ParameterName.IsNone) ? ParameterName : ParameterInfo.Name).Text;

	public FScalarParameterValue(FStructFallback fallback)
	{
		ParameterName = fallback.GetOrDefault<FName>("ParameterName");
		ParameterInfo = fallback.GetOrDefault<FMaterialParameterInfo>("ParameterInfo");
		ParameterValue = fallback.GetOrDefault("ParameterValue", 0f);
		ExpressionGUID = fallback.GetOrDefault<FGuid>("ExpressionGUID");
	}

	public FScalarParameterValue(FAssetArchive Ar)
	{
		ParameterInfo = new FMaterialParameterInfo(Ar);
		ParameterValue = Ar.Read<float>();
		ExpressionGUID = Ar.Read<FGuid>();
	}

	public override string ToString()
	{
		return $"{Name}: {ParameterValue}";
	}
}
