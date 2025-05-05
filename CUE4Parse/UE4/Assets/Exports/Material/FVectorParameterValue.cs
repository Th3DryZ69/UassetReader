using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructFallback]
public class FVectorParameterValue : IUStruct
{
	public readonly FName ParameterName;

	public readonly FMaterialParameterInfo ParameterInfo;

	public readonly FLinearColor? ParameterValue;

	public readonly FGuid ExpressionGUID;

	[JsonIgnore]
	public string Name => ((!ParameterName.IsNone) ? ParameterName : ParameterInfo.Name).Text;

	public FVectorParameterValue(FStructFallback fallback)
	{
		ParameterName = fallback.GetOrDefault<FName>("ParameterName");
		ParameterInfo = fallback.GetOrDefault<FMaterialParameterInfo>("ParameterInfo");
		ParameterValue = fallback.GetOrDefault<FLinearColor>("ParameterValue");
		ExpressionGUID = fallback.GetOrDefault<FGuid>("ExpressionGUID");
	}

	public FVectorParameterValue(FAssetArchive Ar)
	{
		ParameterInfo = new FMaterialParameterInfo(Ar);
		ParameterValue = Ar.Read<FLinearColor>();
		ExpressionGUID = Ar.Read<FGuid>();
	}

	public override string ToString()
	{
		return $"{Name}: {ParameterValue}";
	}
}
