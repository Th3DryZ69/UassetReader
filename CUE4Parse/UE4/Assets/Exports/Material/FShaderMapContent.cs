using System;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderMapContent
{
	public int[] ShaderHash;

	public FHashedName[] ShaderTypes;

	public int[] ShaderPermutations;

	public FShader[] Shaders;

	public FShaderPipeline[] ShaderPipelines;

	public EShaderPlatform ShaderPlatform;

	public FShaderMapContent(FMemoryImageArchive Ar)
	{
		ShaderHash = Ar.ReadHashTable();
		ShaderTypes = Ar.ReadArray<FHashedName>();
		ShaderPermutations = Ar.ReadArray<int>();
		Shaders = Ar.ReadArrayOfPtrs(() => new FShader(Ar));
		ShaderPipelines = Ar.ReadArrayOfPtrs(() => new FShaderPipeline(Ar));
		if (Ar.Game >= EGame.GAME_UE5_2)
		{
			string text = Ar.ReadFString();
			Enum.TryParse<EShaderPlatform>("SP_" + text, out ShaderPlatform);
		}
		else
		{
			ShaderPlatform = Ar.Read<EShaderPlatform>();
			Ar.Position += 7L;
		}
	}
}
