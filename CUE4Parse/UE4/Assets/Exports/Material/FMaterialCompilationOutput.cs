using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialCompilationOutput
{
	public FUniformExpressionSet UniformExpressionSet;

	public uint UsedSceneTextures;

	public byte UsedDBufferTextures;

	public byte RuntimeVirtualTextureOutputAttributeMask;

	public byte b1;

	public byte b2;

	public FMaterialCompilationOutput(FMemoryImageArchive Ar)
	{
		UniformExpressionSet = new FUniformExpressionSet(Ar);
		UsedSceneTextures = Ar.Read<uint>();
		UsedDBufferTextures = Ar.Read<byte>();
		RuntimeVirtualTextureOutputAttributeMask = Ar.Read<byte>();
		b1 = Ar.Read<byte>();
		b2 = Ar.Read<byte>();
	}
}
