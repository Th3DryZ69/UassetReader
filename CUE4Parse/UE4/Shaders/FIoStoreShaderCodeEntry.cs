using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FIoStoreShaderCodeEntry
{
	public readonly long Packed;

	public long Frequency => Packed & 0xF;

	public long ShaderGroupIndex => (Packed & 0x3FFFFFFF0L) >> 4;

	public long UncompressedOffsetInGroup => Packed >> 34;
}
