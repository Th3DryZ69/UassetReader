using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialScalarParameterInfo
{
	public readonly FMemoryImageMaterialParameterInfo ParameterInfo;

	public readonly float DefaultValue;

	public FMaterialScalarParameterInfo(FMemoryImageArchive Ar)
	{
		ParameterInfo = new FMemoryImageMaterialParameterInfo(Ar);
		DefaultValue = Ar.Read<float>();
		Ar.Position += 4L;
	}
}
