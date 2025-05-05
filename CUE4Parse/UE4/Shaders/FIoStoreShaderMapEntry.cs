using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FIoStoreShaderMapEntry
{
	public readonly uint ShaderIndicesOffset;

	public readonly uint NumShaders;
}
