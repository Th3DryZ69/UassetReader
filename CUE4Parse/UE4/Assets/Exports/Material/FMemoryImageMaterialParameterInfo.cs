using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMemoryImageMaterialParameterInfo
{
	public FName Name;

	public int Index;

	public EMaterialParameterAssociation Association;

	public FMemoryImageMaterialParameterInfo(FMemoryImageArchive Ar)
	{
		Name = Ar.ReadFName();
		Index = Ar.Read<int>();
		Association = Ar.Read<EMaterialParameterAssociation>();
		Ar.Position += 3L;
	}
}
