using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialVirtualTextureStack
{
	public uint NumLayers;

	public readonly int[] LayerUniformExpressionIndices = new int[8];

	public int PreallocatedStackTextureIndex;

	public FMaterialVirtualTextureStack(FMemoryImageArchive Ar)
	{
		NumLayers = Ar.Read<uint>();
		Ar.ReadArray(LayerUniformExpressionIndices);
		PreallocatedStackTextureIndex = Ar.Read<int>();
	}
}
