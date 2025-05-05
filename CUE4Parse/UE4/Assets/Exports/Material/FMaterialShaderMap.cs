using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialShaderMap : FShaderMapBase
{
	public FMaterialShaderMapId ShaderMapId;

	public new void Deserialize(FMaterialResourceProxyReader Ar)
	{
		ShaderMapId = new FMaterialShaderMapId(Ar);
		base.Deserialize(Ar);
	}

	protected override FShaderMapContent ReadContent(FMemoryImageArchive Ar)
	{
		return new FMaterialShaderMapContent(Ar);
	}
}
