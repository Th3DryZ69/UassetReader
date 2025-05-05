using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FShaderCodeEntry
{
	public readonly ulong Offset;

	public readonly uint Size;

	public readonly uint UncompressedSize;

	public readonly byte Frequency;
}
