using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialNumericParameterInfo
{
	public FMemoryImageMaterialParameterInfo ParameterInfo;

	public EMaterialParameterType ParameterType;

	public uint DefaultValueOffset;

	public FMaterialNumericParameterInfo(FMemoryImageArchive Ar)
	{
		ParameterInfo = new FMemoryImageMaterialParameterInfo(Ar);
		ParameterType = Ar.Read<EMaterialParameterType>();
		Ar.Position += 3L;
		DefaultValueOffset = Ar.Read<uint>();
	}
}
