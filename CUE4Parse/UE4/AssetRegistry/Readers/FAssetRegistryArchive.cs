using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.AssetRegistry.Objects;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.AssetRegistry.Readers;

public abstract class FAssetRegistryArchive : FArchive
{
	protected readonly FArchive baseArchive;

	public FAssetRegistryHeader Header;

	public FNameEntrySerialized[] NameMap;

	public override bool CanSeek => baseArchive.CanSeek;

	public override long Length => baseArchive.Length;

	public override long Position
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return baseArchive.Position;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			baseArchive.Position = value;
		}
	}

	public override string Name => baseArchive.Name;

	public abstract void SerializeTagsAndBundles(FAssetData assetData);

	public FAssetRegistryArchive(FArchive Ar, FAssetRegistryHeader header)
		: base(Ar.Versions)
	{
		baseArchive = Ar;
		Header = header;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int Read(byte[] buffer, int offset, int count)
	{
		return baseArchive.Read(buffer, offset, count);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override long Seek(long offset, SeekOrigin origin)
	{
		return baseArchive.Seek(offset, origin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override T Read<T>()
	{
		return baseArchive.Read<T>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override byte[] ReadBytes(int length)
	{
		return baseArchive.ReadBytes(length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override void Serialize(byte* ptr, int length)
	{
		baseArchive.Serialize(ptr, length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override T[] ReadArray<T>(int length)
	{
		return baseArchive.ReadArray<T>(length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ReadArray<T>(T[] array)
	{
		baseArchive.ReadArray(array);
	}
}
