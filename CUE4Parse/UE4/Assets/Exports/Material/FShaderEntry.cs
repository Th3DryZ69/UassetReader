using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FShaderEntry
{
	public byte[] Code;

	public int UncompressedSize;

	public byte Frequency;

	public FShaderEntry(FArchive Ar)
	{
		Code = Ar.ReadArray<byte>();
		UncompressedSize = Ar.Read<int>();
		Frequency = Ar.Read<byte>();
	}
}
