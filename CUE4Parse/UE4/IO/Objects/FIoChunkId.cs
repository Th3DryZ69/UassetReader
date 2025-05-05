using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.IO.Objects;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct FIoChunkId : IEquatable<FIoChunkId>
{
	public readonly ulong ChunkId;

	private readonly ushort _chunkIndex;

	private readonly byte _padding;

	public readonly byte ChunkType;

	public FIoChunkId(ulong chunkId, ushort chunkIndex, byte chunkType)
	{
		ChunkId = chunkId;
		_chunkIndex = (ushort)(((chunkIndex & 0xFF) << 8) | ((chunkIndex & 0xFF00) >> 8));
		ChunkType = chunkType;
		_padding = 0;
	}

	public FIoChunkId(ulong chunkId, ushort chunkIndex, EIoChunkType chunkType)
		: this(chunkId, chunkIndex, (byte)chunkType)
	{
	}

	public FIoChunkId(ulong chunkId, ushort chunkIndex, EIoChunkType5 chunkType)
		: this(chunkId, chunkIndex, (byte)chunkType)
	{
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FPackageId AsPackageId()
	{
		return new FPackageId(ChunkId);
	}

	public unsafe ulong HashWithSeed(int seed)
	{
		fixed (FIoChunkId* ptr = &this)
		{
			int num = sizeof(FIoChunkId);
			ulong num2 = ((seed != 0) ? ((ulong)seed) : 14695981039346656037uL);
			for (int i = 0; i < num; i++)
			{
				num2 = (num2 * 1099511628211L) ^ ((byte*)ptr)[i];
			}
			return num2;
		}
	}

	public static bool operator ==(FIoChunkId left, FIoChunkId right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FIoChunkId left, FIoChunkId right)
	{
		return !left.Equals(right);
	}

	public bool Equals(FIoChunkId other)
	{
		if (ChunkId == other.ChunkId)
		{
			return ChunkType == other.ChunkType;
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FIoChunkId other)
		{
			return Equals(other);
		}
		return false;
	}

	public unsafe override int GetHashCode()
	{
		fixed (FIoChunkId* ptr = &this)
		{
			int num = sizeof(FIoChunkId);
			int num2 = 5381;
			for (int i = 0; i < num; i++)
			{
				num2 = num2 * 33 + ((byte*)ptr)[i];
			}
			return num2;
		}
	}

	public override string ToString()
	{
		return $"0x{ChunkId:X8} | {ChunkType}";
	}
}
