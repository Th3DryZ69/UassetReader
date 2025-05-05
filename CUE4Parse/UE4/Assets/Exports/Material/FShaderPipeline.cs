using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderPipeline
{
	public enum EFilter
	{
		EAll,
		EOnlyShared,
		EOnlyUnique
	}

	private const int SF_NumGraphicsFrequencies = 5;

	public FHashedName TypeName;

	public FShader[] Shaders;

	public int[] PermutationIds;

	public FShaderPipeline(FMemoryImageArchive Ar)
	{
		TypeName = new FHashedName(Ar);
		Shaders = new FShader[5];
		for (int i = 0; i < Shaders.Length; i++)
		{
			long position = Ar.Position;
			FFrozenMemoryImagePtr fFrozenMemoryImagePtr = new FFrozenMemoryImagePtr(Ar);
			if (fFrozenMemoryImagePtr.IsFrozen)
			{
				Ar.Position = position + fFrozenMemoryImagePtr.OffsetFromThis;
				Shaders[i] = new FShader(Ar);
			}
			Ar.Position = (position + 8).Align(8);
		}
		PermutationIds = Ar.ReadArray<int>(5);
	}
}
