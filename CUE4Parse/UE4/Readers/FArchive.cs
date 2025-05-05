#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.Compression;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Exports.Texture;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Serilog;

namespace CUE4Parse.UE4.Readers;

public abstract class FArchive : Stream, ICloneable
{
	private struct FCompressedChunkInfo
	{
		public long CompressedSize;

		public long UncompressedSize;
	}

	public VersionContainer Versions;

	public EGame Game
	{
		get
		{
			return Versions.Game;
		}
		set
		{
			Versions.Game = value;
		}
	}

	public FPackageFileVersion Ver
	{
		get
		{
			return Versions.Ver;
		}
		set
		{
			Versions.Ver = value;
		}
	}

	public ETexturePlatform Platform
	{
		get
		{
			return Versions.Platform;
		}
		set
		{
			Versions.Platform = value;
		}
	}

	public abstract string Name { get; }

	public override bool CanRead { get; } = true;

	public override bool CanWrite { get; }

	public virtual byte[] ReadBytes(int length)
	{
		byte[] array = new byte[length];
		Read(array, 0, length);
		return array;
	}

	public unsafe virtual void Serialize(byte* ptr, int length)
	{
		byte[] array = ReadBytes(length);
		Unsafe.CopyBlockUnaligned(ref *ptr, ref array[0], (uint)length);
	}

	public virtual T Read<T>()
	{
		int length = Unsafe.SizeOf<T>();
		return Unsafe.ReadUnaligned<T>(ref ReadBytes(length)[0]);
	}

	public virtual T[] ReadArray<T>(int length)
	{
		int num = Unsafe.SizeOf<T>();
		byte[] array = ReadBytes(num * length);
		T[] array2 = new T[length];
		if (length > 0)
		{
			Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref array2[0]), ref array[0], (uint)(length * num));
		}
		return array2;
	}

	public virtual void ReadArray<T>(T[] array)
	{
		if (array.Length != 0)
		{
			int num = Unsafe.SizeOf<T>();
			byte[] array2 = ReadBytes(num * array.Length);
			Unsafe.CopyBlockUnaligned(ref Unsafe.As<T, byte>(ref array[0]), ref array2[0], (uint)(array.Length * num));
		}
	}

	protected FArchive(VersionContainer? versions = null)
	{
		Versions = versions ?? new VersionContainer();
	}

	public override void Flush()
	{
	}

	public override void SetLength(long value)
	{
		throw new InvalidOperationException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new InvalidOperationException();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ReadArray<T>(T[] array, Func<T> getter)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = getter();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T[] ReadArray<T>(int length, Func<T> getter)
	{
		T[] array = new T[length];
		if (length == 0)
		{
			return array;
		}
		ReadArray(array, getter);
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual T[] ReadArray<T>(Func<T> getter)
	{
		int length = Read<int>();
		return ReadArray(length, getter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual T[] ReadArray<T>() where T : struct
	{
		int num = Read<int>();
		if (num <= 0)
		{
			return Array.Empty<T>();
		}
		return ReadArray<T>(num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T[] ReadBulkArray<T>(int elementSize, int elementCount, Func<T> getter)
	{
		long position = Position;
		T[] array = ReadArray(elementCount, getter);
		if (Game != EGame.GAME_HogwartsLegacy && Position != position + array.Length * elementSize)
		{
			throw new ParserException($"RawArray item size mismatch: expected {elementSize}, serialized {(Position - position) / array.Length}");
		}
		return array;
	}

	public T[] ReadBulkArray<T>() where T : struct
	{
		int num = Read<int>();
		int num2 = Read<int>();
		if (num2 == 0)
		{
			return Array.Empty<T>();
		}
		long position = Position;
		T[] array = ReadArray<T>(num2);
		if (Position != position + array.Length * num)
		{
			throw new ParserException($"RawArray item size mismatch: expected {num}, serialized {(Position - position) / array.Length}");
		}
		return array;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public T[] ReadBulkArray<T>(Func<T> getter)
	{
		int elementSize = Read<int>();
		int elementCount = Read<int>();
		return ReadBulkArray(elementSize, elementCount, getter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SkipBulkArrayData()
	{
		int num = Read<int>();
		int num2 = Read<int>();
		Position += num * num2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SkipFixedArray(int size = -1)
	{
		int num = Read<int>();
		Position += num * size;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Dictionary<TKey, TValue> ReadMap<TKey, TValue>(int length, Func<(TKey, TValue)> getter) where TKey : notnull
	{
		Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(length);
		for (int i = 0; i < length; i++)
		{
			(TKey, TValue) tuple = getter();
			TKey item = tuple.Item1;
			TValue item2 = tuple.Item2;
			dictionary[item] = item2;
		}
		return dictionary;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Dictionary<TKey, TValue> ReadMap<TKey, TValue>(Func<(TKey, TValue)> getter) where TKey : notnull
	{
		int length = Read<int>();
		return ReadMap(length, getter);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ReadBoolean()
	{
		int num = Read<int>();
		return num switch
		{
			0 => false, 
			1 => true, 
			_ => throw new ParserException(this, $"Invalid bool value ({num})"), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool ReadFlag()
	{
		byte b = Read<byte>();
		return b switch
		{
			0 => false, 
			1 => true, 
			_ => throw new ParserException(this, $"Invalid bool value ({b})"), 
		};
	}

	public virtual uint ReadIntPacked()
	{
		uint num = 0u;
		byte b = 0;
		bool flag = true;
		while (flag)
		{
			byte b2 = Read<byte>();
			flag = (b2 & 1) != 0;
			b2 >>= 1;
			num += (uint)(b2 << 7 * b++);
		}
		return num;
	}

	public unsafe virtual void SerializeBits(void* v, long lengthBits)
	{
		Serialize((byte*)v, (int)((lengthBits + 7) / 8));
		if (lengthBits % 8 != 0L)
		{
			byte* num = (byte*)v + lengthBits / 8;
			*num &= (byte)((1 << (int)(lengthBits & 7)) - 1);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Read7BitEncodedInt()
	{
		int num = 0;
		int num2 = 0;
		byte b;
		do
		{
			if (num2 == 35)
			{
				throw new FormatException("Stream is corrupted");
			}
			b = Read<byte>();
			num |= (b & 0x7F) << num2;
			num2 += 7;
		}
		while ((b & 0x80) != 0);
		return num;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe string ReadString()
	{
		int num = Read7BitEncodedInt();
		if (num <= 0)
		{
			return string.Empty;
		}
		byte* ptr = stackalloc byte[(int)(uint)num];
		Serialize(ptr, num);
		return new string((sbyte*)ptr, 0, num);
	}

	public unsafe virtual string ReadFString()
	{
		int num = Read<int>();
		if (num == int.MinValue)
		{
			throw new ArgumentOutOfRangeException("length", "Archive is corrupted");
		}
		if (num == 0)
		{
			return string.Empty;
		}
		Span<byte> span;
		if (num < 0)
		{
			num = -num;
			int num2 = num * 2;
			span = ((num2 > 1024) ? ((Span<byte>)new byte[num2]) : stackalloc byte[num2]);
			Span<byte> span2 = span;
			fixed (byte* ptr = span2)
			{
				Serialize(ptr, num2);
				if (span2[num2 - 1] != 0 || span2[num2 - 2] != 0)
				{
					throw new ParserException(this, "Serialized FString is not null terminated");
				}
				return new string((char*)ptr, 0, num - 1);
			}
		}
		span = ((num > 1024) ? ((Span<byte>)new byte[num]) : stackalloc byte[num]);
		Span<byte> span3 = span;
		fixed (byte* ptr2 = span3)
		{
			Serialize(ptr2, num);
			if (span3[num - 1] != 0)
			{
				throw new ParserException(this, "Serialized FString is not null terminated");
			}
			return new string((sbyte*)ptr2, 0, num - 1);
		}
	}

	public virtual FName ReadFName()
	{
		return new FName(ReadFString());
	}

	public virtual UObject? ReadUObject()
	{
		throw new InvalidOperationException("Generic FArchive can't read UObject's");
	}

	public void SerializeCompressedNew(byte[] dest, int length, string compressionFormatToDecodeOldV1Files, ECompressionFlags flags, bool bTreatBufferAsFileReader, out long outPartialReadLength)
	{
		FCompressedChunkInfo fCompressedChunkInfo = Read<FCompressedChunkInfo>();
		bool flag = false;
		bool flag2 = false;
		if (fCompressedChunkInfo.CompressedSize != 2653586369u)
		{
			if (fCompressedChunkInfo.CompressedSize != 3246598814u && fCompressedChunkInfo.CompressedSize != (long)BYTESWAP_ORDER64(2653586369uL))
			{
				if (fCompressedChunkInfo.CompressedSize == 2459565878575530945L || fCompressedChunkInfo.CompressedSize == (long)BYTESWAP_ORDER64(2459565878575530945uL))
				{
					flag = fCompressedChunkInfo.CompressedSize != 2459565878575530945L;
					flag2 = true;
					throw new NotImplementedException();
				}
				throw new ParserException(this, "BulkData compressed header read error. This package may be corrupt!");
			}
			flag = true;
		}
		if (!flag2 && flags.HasFlag(ECompressionFlags.COMPRESS_DeprecatedFormatFlagsMask))
		{
			Log.Warning("Old style compression flags are being used with FAsyncCompressionChunk, please update any code using this!");
			throw new NotImplementedException();
		}
		if (!Enum.TryParse<CompressionMethod>(compressionFormatToDecodeOldV1Files, out var result))
		{
			throw new ParserException(this, "BulkData compressed header read error. This package may be corrupt!\nCompressionFormatToDecode not found : " + compressionFormatToDecodeOldV1Files);
		}
		FCompressedChunkInfo fCompressedChunkInfo2 = Read<FCompressedChunkInfo>();
		if (flag)
		{
			fCompressedChunkInfo2.CompressedSize = (long)BYTESWAP_ORDER64((ulong)fCompressedChunkInfo2.CompressedSize);
			fCompressedChunkInfo2.UncompressedSize = (long)BYTESWAP_ORDER64((ulong)fCompressedChunkInfo2.UncompressedSize);
			fCompressedChunkInfo.UncompressedSize = (long)BYTESWAP_ORDER64((ulong)fCompressedChunkInfo.UncompressedSize);
		}
		long num = fCompressedChunkInfo.UncompressedSize;
		if (num == 2653586369u)
		{
			num = 131072L;
		}
		Trace.Assert(num > 0);
		Trace.Assert(fCompressedChunkInfo2.UncompressedSize <= length);
		Trace.Assert(fCompressedChunkInfo2.UncompressedSize >= 0);
		if (fCompressedChunkInfo2.UncompressedSize > length)
		{
			throw new ParserException(this, $"Archive SerializedCompressed UncompressedSize ({fCompressedChunkInfo2.UncompressedSize}) > Length ({length})");
		}
		outPartialReadLength = fCompressedChunkInfo2.UncompressedSize;
		long num2 = (fCompressedChunkInfo2.UncompressedSize + num - 1) / num;
		FCompressedChunkInfo[] array = new FCompressedChunkInfo[num2];
		long num3 = 0L;
		long num4 = 0L;
		long num5 = 0L;
		for (int i = 0; i < num2; i++)
		{
			array[i] = Read<FCompressedChunkInfo>();
			if (flag)
			{
				array[i].CompressedSize = (long)BYTESWAP_ORDER64((ulong)array[i].CompressedSize);
				array[i].UncompressedSize = (long)BYTESWAP_ORDER64((ulong)array[i].UncompressedSize);
			}
			num3 = Math.Max(array[i].CompressedSize, num3);
			num4 += array[i].CompressedSize;
			num5 += array[i].UncompressedSize;
		}
		if (num4 != fCompressedChunkInfo2.CompressedSize)
		{
			throw new ParserException(this, $"Archive SerializedCompressed TotalChunkCompressedSize ({num4}) != Summary.CompressedSize ({fCompressedChunkInfo2.CompressedSize})");
		}
		if (num5 != fCompressedChunkInfo2.UncompressedSize)
		{
			throw new ParserException(this, $"Archive SerializedCompressed TotalChunkUncompressedSize ({num5}) != Summary.UnompressedSize ({fCompressedChunkInfo2.UncompressedSize})");
		}
		Trace.Assert(!bTreatBufferAsFileReader);
		int num6 = 0;
		byte[] array2 = new byte[num3];
		for (int j = 0; j < num2; j++)
		{
			ref FCompressedChunkInfo reference = ref array[j];
			Read(array2, 0, (int)reference.CompressedSize);
			try
			{
				CUE4Parse.Compression.Compression.Decompress(array2, 0, (int)reference.CompressedSize, dest, num6, (int)reference.UncompressedSize, result);
			}
			catch (Exception innerException)
			{
				throw new ParserException(this, "Failed to uncompress data in " + Name + ", CompressionFormatToDecode=" + compressionFormatToDecodeOldV1Files, innerException);
			}
			num6 += (int)reference.UncompressedSize;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ulong BYTESWAP_ORDER64(ulong value)
	{
		value = ((value << 8) & 0xFF00FF00FF00FF00uL) | ((value >> 8) & 0xFF00FF00FF00FFL);
		value = ((value << 16) & 0xFFFF0000FFFF0000uL) | ((value >> 16) & 0xFFFF0000FFFFL);
		return (value << 32) | (value >> 32);
	}

	public abstract object Clone();
}
