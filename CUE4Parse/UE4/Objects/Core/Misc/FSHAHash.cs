using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.Objects.Core.Misc;

public readonly struct FSHAHash : IUStruct
{
	public const int SIZE = 20;

	public readonly byte[] Hash;

	public FSHAHash(FArchive Ar)
	{
		Hash = Ar.ReadBytes(20);
	}

	public unsafe override string ToString()
	{
		fixed (byte* hash = Hash)
		{
			return UnsafePrint.BytesToHex(hash, 20u);
		}
	}
}
