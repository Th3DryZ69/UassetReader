using System;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.IO.Objects;

public class FFilePackageStoreEntry
{
	public int ExportCount;

	public int ExportBundleCount;

	public FPackageId[] ImportedPackages;

	public FSHAHash[] ShaderMapHashes;

	public FFilePackageStoreEntry(FArchive Ar)
	{
		if (Ar.Game >= EGame.GAME_UE5_0)
		{
			if (Ar.Game < EGame.GAME_UE5_3)
			{
				ExportCount = Ar.Read<int>();
				ExportBundleCount = Ar.Read<int>();
			}
			ImportedPackages = ReadCArrayView<FPackageId>(Ar);
			ShaderMapHashes = ReadCArrayView(Ar, () => new FSHAHash(Ar));
		}
		else
		{
			Ar.Position += 8L;
			ExportCount = Ar.Read<int>();
			ExportBundleCount = Ar.Read<int>();
			Ar.Position += 8L;
			ImportedPackages = ReadCArrayView<FPackageId>(Ar);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static T[] ReadCArrayView<T>(FArchive Ar) where T : struct
	{
		long position = Ar.Position;
		int num = Ar.Read<int>();
		int num2 = Ar.Read<int>();
		if (num == 0)
		{
			return Array.Empty<T>();
		}
		long position2 = Ar.Position;
		Ar.Position = position + num2;
		T[] result = Ar.ReadArray<T>(num);
		Ar.Position = position2;
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static T[] ReadCArrayView<T>(FArchive Ar, Func<T> getter)
	{
		long position = Ar.Position;
		int num = Ar.Read<int>();
		int num2 = Ar.Read<int>();
		if (num == 0)
		{
			return Array.Empty<T>();
		}
		long position2 = Ar.Position;
		Ar.Position = position + num2;
		T[] result = Ar.ReadArray(num, getter);
		Ar.Position = position2;
		return result;
	}
}
