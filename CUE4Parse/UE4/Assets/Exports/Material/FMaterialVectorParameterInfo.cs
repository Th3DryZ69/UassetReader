using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialVectorParameterInfo
{
	public readonly FMemoryImageMaterialParameterInfo ParameterInfo;

	public readonly FLinearColor DefaultValue;

	public FMaterialVectorParameterInfo(FMemoryImageArchive Ar)
	{
		ParameterInfo = new FMemoryImageMaterialParameterInfo(Ar);
		DefaultValue = Ar.Read<FLinearColor>();
	}
}
