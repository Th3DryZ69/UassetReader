using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.MappingsProvider.Usmap;

public static class UsmapArchiveExtensions
{
	private const int InvalidNameIndex = -1;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string? ReadName(this FArchive Ar, IReadOnlyList<string> nameLut)
	{
		int num = Ar.ReadNameEntry();
		if (num == -1)
		{
			return null;
		}
		return nameLut[num];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int ReadNameEntry(this FArchive Ar)
	{
		return Ar.Read<int>();
	}

	public unsafe static string ReadStringUnsafe(this FArchive Ar, int nameLength)
	{
		byte* ptr = stackalloc byte[(int)(uint)nameLength];
		Ar.Serialize(ptr, nameLength);
		return new string((sbyte*)ptr, 0, nameLength);
	}
}
