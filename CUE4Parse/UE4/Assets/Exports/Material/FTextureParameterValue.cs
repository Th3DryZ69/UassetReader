using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

[StructFallback]
public class FTextureParameterValue : IUStruct
{
	public readonly FName ParameterName;

	public readonly FMaterialParameterInfo ParameterInfo;

	public readonly FPackageIndex ParameterValue;

	public readonly FGuid ExpressionGUID;

	[JsonIgnore]
	public string Name => ((!ParameterName.IsNone) ? ParameterName : ParameterInfo.Name).Text;

	public FTextureParameterValue(FStructFallback fallback)
	{
		ParameterName = fallback.GetOrDefault<FName>("ParameterName");
		ParameterInfo = fallback.GetOrDefault<FMaterialParameterInfo>("ParameterInfo");
		ParameterValue = fallback.GetOrDefault<FPackageIndex>("ParameterValue");
		ExpressionGUID = fallback.GetOrDefault<FGuid>("ExpressionGUID");
	}

	public FTextureParameterValue(FAssetArchive Ar)
	{
		ParameterInfo = new FMaterialParameterInfo(Ar);
		ParameterValue = new FPackageIndex(Ar);
		ExpressionGUID = Ar.Read<FGuid>();
	}

	public override string ToString()
	{
		return $"{Name}: {ParameterValue}";
	}
}
