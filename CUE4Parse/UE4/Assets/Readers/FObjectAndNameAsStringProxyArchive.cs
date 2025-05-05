using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Assets.Readers;

public class FObjectAndNameAsStringProxyArchive : FAssetArchive
{
	protected readonly FArchive InnerArchive;

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

	public FObjectAndNameAsStringProxyArchive(FAssetArchive Ar)
		: this(Ar, Ar.Owner, Ar.AbsoluteOffset)
	{
		InnerArchive = Ar;
	}

	public FObjectAndNameAsStringProxyArchive(FArchive Ar)
		: base(Ar, null)
	{
		InnerArchive = Ar;
	}

	public FObjectAndNameAsStringProxyArchive(FArchive Ar, IPackage owner, int absoluteOffset = 0)
		: base(Ar, owner, absoluteOffset)
	{
		InnerArchive = Ar;
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
		return new FObjectAndNameAsStringProxyArchive((FArchive)InnerArchive.Clone());
	}

	public override FName ReadFName()
	{
		return ReadFString();
	}

    public override Lazy<T?> ReadObject<T>() where T : class
    {
        var path = ReadFString();
        return new Lazy<T?>(() =>
        {
            Debug.Assert(Owner.Provider != null, "Owner.Provider != null");
            return Owner.Provider.TryLoadObject<T>(path, out var obj) ? obj : null;
        });
    }
}
