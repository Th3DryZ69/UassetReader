using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialTextureParameterInfo
{
	public FMemoryImageMaterialParameterInfo ParameterInfo;

	public int TextureIndex = -1;

	public ESamplerSourceMode SamplerSource;

	public byte VirtualTextureLayerIndex;

	public FMaterialTextureParameterInfo(FMemoryImageArchive Ar)
	{
		ParameterInfo = new FMemoryImageMaterialParameterInfo(Ar);
		TextureIndex = Ar.Read<int>();
		SamplerSource = Ar.Read<ESamplerSourceMode>();
		VirtualTextureLayerIndex = Ar.Read<byte>();
		Ar.Position += 2L;
	}
}
