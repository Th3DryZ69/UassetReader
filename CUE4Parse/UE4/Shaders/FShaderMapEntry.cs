using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FShaderMapEntry
{
	public readonly uint ShaderIndicesOffset;

	public readonly uint NumShaders;

	public readonly uint FirstPreloadIndex;

	public readonly uint NumPreloadEntries;
}
