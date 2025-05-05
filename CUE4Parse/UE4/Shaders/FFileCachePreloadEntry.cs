using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FFileCachePreloadEntry
{
	public readonly long Offset;

	public readonly long Size;
}
