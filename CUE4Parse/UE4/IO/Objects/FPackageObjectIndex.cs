using System;

namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FPackageObjectIndex : IEquatable<FPackageObjectIndex>
{
	public const int Size = 8;

	public const int IndexBits = 62;

	public const ulong IndexMask = 4611686018427387903uL;

	public const ulong TypeMask = 13835058055282163712uL;

	public const int TypeShift = 62;

	public const ulong Invalid = ulong.MaxValue;

	public readonly ulong TypeAndId;

	public EType Type => (EType)(TypeAndId >> 62);

	public ulong Value => TypeAndId & 0x3FFFFFFFFFFFFFFFL;

	public bool IsNull => TypeAndId == ulong.MaxValue;

	public bool IsExport => Type == EType.Export;

	public bool IsImport
	{
		get
		{
			if (!IsScriptImport)
			{
				return IsPackageImport;
			}
			return true;
		}
	}

	public bool IsScriptImport => Type == EType.ScriptImport;

	public bool IsPackageImport => Type == EType.PackageImport;

	public uint AsExport => (uint)TypeAndId;

	public FPackageImportReference AsPackageImportRef => new FPackageImportReference
	{
		ImportedPackageIndex = (uint)((TypeAndId & 0x3FFFFFFFFFFFFFFFL) >> 32),
		ImportedPublicExportHashIndex = (uint)TypeAndId
	};

	public FPackageObjectIndex(ulong typeAndId)
	{
		TypeAndId = typeAndId;
	}

	public bool Equals(FPackageObjectIndex other)
	{
		return TypeAndId == other.TypeAndId;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FPackageObjectIndex other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return TypeAndId.GetHashCode();
	}

	public static bool operator ==(FPackageObjectIndex left, FPackageObjectIndex right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FPackageObjectIndex left, FPackageObjectIndex right)
	{
		return !left.Equals(right);
	}
}
