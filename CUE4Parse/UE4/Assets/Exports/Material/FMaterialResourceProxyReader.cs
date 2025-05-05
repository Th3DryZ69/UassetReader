using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialResourceProxyReader : FArchive
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct FMaterialResourceLocOnDisk
	{
		public readonly uint Offset;

		public readonly ERHIFeatureLevel FeatureLevel;

		public readonly EMaterialQualityLevel QualityLevel;
	}

	protected readonly FArchive InnerArchive;

	public readonly FNameEntrySerialized[] NameMap;

	public FMaterialResourceLocOnDisk[] Locs;

	private long _offsetToFirstResource;

	public override bool CanSeek => InnerArchive.CanSeek;

	public override long Length => InnerArchive.Length;

	public override long Position
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return InnerArchive.Position;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			InnerArchive.Position = value;
		}
	}

	public override string Name => InnerArchive.Name;

	public FMaterialResourceProxyReader(FArchive Ar)
		: base(Ar.Versions)
	{
		InnerArchive = Ar;
		NameMap = InnerArchive.ReadArray(() => new FNameEntrySerialized(Ar));
		Locs = InnerArchive.ReadArray<FMaterialResourceLocOnDisk>();
		InnerArchive.Read<int>();
		_offsetToFirstResource = InnerArchive.Position;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override FName ReadFName()
	{
		int num = InnerArchive.Read<int>();
		int number = InnerArchive.Read<int>();
		if (num < 0 || num >= NameMap.Length)
		{
			throw new ParserException(InnerArchive, $"FName could not be read, requested index {num}, name map size {NameMap.Length}");
		}
		return new FName(NameMap[num], num, number);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int Read(byte[] buffer, int offset, int count)
	{
		return InnerArchive.Read(buffer, offset, count);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override long Seek(long offset, SeekOrigin origin)
	{
		return InnerArchive.Seek(offset, origin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override T Read<T>()
	{
		return InnerArchive.Read<T>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override byte[] ReadBytes(int length)
	{
		return InnerArchive.ReadBytes(length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override void Serialize(byte* ptr, int length)
	{
		InnerArchive.Serialize(ptr, length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override T[] ReadArray<T>(int length)
	{
		return InnerArchive.ReadArray<T>(length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ReadArray<T>(T[] array)
	{
		InnerArchive.ReadArray(array);
	}

	public override object Clone()
	{
		return new FMaterialResourceProxyReader((FArchive)InnerArchive.Clone());
	}
}
