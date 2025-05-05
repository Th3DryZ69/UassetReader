using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FIoStoreShaderGroupEntry
{
	public readonly uint ShaderIndicesOffset;

	public readonly uint NumShaders;

	public readonly uint UncompressedSize;

	public readonly uint CompressedSize;
}
