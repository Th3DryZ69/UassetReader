using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FIoOffsetAndLength
{
	public readonly ulong Offset;

	public readonly ulong Length;

	public readonly long Position;

	public readonly byte[] Buffer;

	public unsafe FIoOffsetAndLength(FArchive Ar)
	{
		Position = Ar.Position;
		Buffer = Ar.ReadBytes(12);
		Ar.Position = Position;
		byte* ptr = stackalloc byte[10];
		Ar.Serialize(ptr, 10);
		Offset = ptr[4] | ((ulong)ptr[3] << 8) | ((ulong)ptr[2] << 16) | ((ulong)ptr[1] << 24) | ((ulong)(*ptr) << 32);
		Length = ptr[9] | ((ulong)ptr[8] << 8) | ((ulong)ptr[7] << 16) | ((ulong)ptr[6] << 24) | ((ulong)ptr[5] << 32);
	}

	public override string ToString()
	{
		return $"{"Offset"} {Offset} | {"Length"} {Length}";
	}
}
