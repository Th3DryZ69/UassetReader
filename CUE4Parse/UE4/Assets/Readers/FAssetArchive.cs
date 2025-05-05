using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using Serilog;

namespace CUE4Parse.UE4.Assets.Readers;

public class FAssetArchive : FArchive
{
	private readonly Dictionary<PayloadType, Lazy<FAssetArchive?>> _payloads;

	private readonly FArchive _baseArchive;

	public readonly IPackage Owner;

	public int AbsoluteOffset;

	public bool HasUnversionedProperties => Owner.HasFlags(EPackageFlags.PKG_UnversionedProperties);

	public bool IsFilterEditorOnly => Owner.HasFlags(EPackageFlags.PKG_FilterEditorOnly);

	public override bool CanSeek => _baseArchive.CanSeek;

	public override long Length => _baseArchive.Length;

	public long AbsolutePosition => AbsoluteOffset + Position;

	public override long Position
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _baseArchive.Position;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			_baseArchive.Position = value;
		}
	}

	public override string Name => _baseArchive.Name;

	public FAssetArchive(FArchive baseArchive, IPackage owner, int absoluteOffset = 0, Dictionary<PayloadType, Lazy<FAssetArchive?>>? payloads = null)
		: base(baseArchive.Versions)
	{
		_payloads = payloads ?? new Dictionary<PayloadType, Lazy<FAssetArchive>>();
		_baseArchive = baseArchive;
		Owner = owner;
		AbsoluteOffset = absoluteOffset;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override FName ReadFName()
	{
		int num = Read<int>();
		int number = Read<int>();
		if (num < 0 || num >= Owner.NameMap.Length)
		{
			throw new ParserException(this, $"FName could not be read, requested index {num}, name map size {Owner.NameMap.Length}");
		}
		return new FName(Owner.NameMap[num], num, number);
	}

	public virtual Lazy<T?> ReadObject<T>() where T : UObject
	{
		FPackageIndex index = new FPackageIndex(this);
		ResolvedObject resolved = index.ResolvedObject;
		return new Lazy<T>(delegate
		{
			if (resolved == null)
			{
				if (index.IsNull)
				{
					return (T?)null;
				}
				Log.Warning("Failed to resolve index {Index}", index);
				return (T?)null;
			}
			if (Owner.Provider == null)
			{
				Log.Warning("Can't load object {Resolved} without a file provider", resolved.Name);
				return (T?)null;
			}
			if (!resolved.TryLoad(out UObject export))
			{
				Log.Warning("Failed to load object {Obj}", resolved.Name);
				return (T?)null;
			}
			if (export is T result)
			{
				return result;
			}
			Log.Warning("Object has unexpected type {ObjType}, expected type {Type}", export.GetType().Name, typeof(T).Name);
			return (T?)null;
		});
	}

	public override UObject? ReadUObject()
	{
		return ReadObject<UObject>().Value;
	}

	public bool TryGetPayload(PayloadType type, out FAssetArchive? ar)
	{
		ar = null;
		if (!_payloads.TryGetValue(type, out Lazy<FAssetArchive> value))
		{
			return false;
		}
		ar = value.Value;
		return true;
	}

	public FAssetArchive GetPayload(PayloadType type)
	{
		_payloads.TryGetValue(type, out Lazy<FAssetArchive> value);
		return value?.Value ?? throw new ParserException(this, $"{type} is needed to parse the current package");
	}

	public void AddPayload(PayloadType type, FAssetArchive payload)
	{
		if (_payloads.ContainsKey(type))
		{
			throw new ParserException(this, $"Can't add a payload that is already attached of type {type}");
		}
		_payloads[type] = new Lazy<FAssetArchive>(() => payload);
	}

	public void AddPayload(PayloadType type, int absoluteOffset, Lazy<FArchive?> payload)
	{
		if (_payloads.ContainsKey(type))
		{
			throw new ParserException(this, $"Can't add a payload that is already attached of type {type}");
		}
		_payloads[type] = new Lazy<FAssetArchive>(delegate
		{
			FArchive value = payload.Value;
			return (value != null) ? new FAssetArchive(value, Owner, absoluteOffset) : null;
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int Read(byte[] buffer, int offset, int count)
	{
		return _baseArchive.Read(buffer, offset, count);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override long Seek(long offset, SeekOrigin origin)
	{
		return _baseArchive.Seek(offset, origin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long SeekAbsolute(long offset, SeekOrigin origin)
	{
		return _baseArchive.Seek(offset - AbsoluteOffset, origin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override T Read<T>()
	{
		return _baseArchive.Read<T>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override byte[] ReadBytes(int length)
	{
		return _baseArchive.ReadBytes(length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe override void Serialize(byte* ptr, int length)
	{
		_baseArchive.Serialize(ptr, length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override T[] ReadArray<T>(int length)
	{
		return _baseArchive.ReadArray<T>(length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void ReadArray<T>(T[] array)
	{
		_baseArchive.ReadArray(array);
	}

	public override object Clone()
	{
		return new FAssetArchive((FArchive)_baseArchive.Clone(), Owner, AbsoluteOffset, _payloads);
	}
}
