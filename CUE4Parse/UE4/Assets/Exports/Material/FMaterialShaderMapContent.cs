using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialShaderMapContent : FShaderMapContent
{
	public FMeshMaterialShaderMap[] OrderedMeshShaderMaps;

	public FMaterialCompilationOutput MaterialCompilationOutput;

	public FSHAHash ShaderContentHash;

	public FMaterialShaderMapContent(FMemoryImageArchive Ar)
		: base(Ar)
	{
		OrderedMeshShaderMaps = Ar.ReadArrayOfPtrs(() => new FMeshMaterialShaderMap(Ar));
		MaterialCompilationOutput = new FMaterialCompilationOutput(Ar);
		ShaderContentHash = new FSHAHash(Ar);
	}
}
