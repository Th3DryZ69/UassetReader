using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMeshMaterialShaderMap : FShaderMapContent
{
	public FHashedName VertexFactoryTypeName;

	public FMeshMaterialShaderMap(FMemoryImageArchive Ar)
		: base(Ar)
	{
		VertexFactoryTypeName = Ar.Read<FHashedName>();
	}
}
