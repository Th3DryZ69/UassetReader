using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FIoStoreTocEntryMeta
{
	public readonly FIoChunkHash ChunkHash;

	public readonly FIoStoreTocEntryMetaFlags Flags;

	public FIoStoreTocEntryMeta(FArchive Ar)
	{
		ChunkHash = new FIoChunkHash(Ar);
		Flags = Ar.Read<FIoStoreTocEntryMetaFlags>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override string ToString()
	{
		return $"{Flags} | {UnsafePrint.BytesToHex((byte*)Unsafe.AsPointer(ref Unsafe.AsRef(in ChunkHash.Hash)), 32u)}";
	}
}
