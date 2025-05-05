using System;
using System.Runtime.InteropServices;

namespace CUE4Parse.UE4.Objects.UObject;

[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
public readonly struct FSerializedNameHeader : IEquatable<FSerializedNameHeader>
{
	public const int Size = 2;

	private readonly byte _data0;

	private readonly byte _data1;

	public bool IsUtf16 => (_data0 & 0x80) != 0;

	public uint Length => (uint)(((_data0 & 0x7F) << 8) + _data1);

	public bool Equals(FSerializedNameHeader other)
	{
		if (_data0 == other._data0)
		{
			return _data1 == other._data1;
		}
		return false;
	}

	public override bool Equals(object? obj)
	{
		if (obj is FSerializedNameHeader other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(_data0, _data1);
	}

	public static bool operator ==(FSerializedNameHeader left, FSerializedNameHeader right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FSerializedNameHeader left, FSerializedNameHeader right)
	{
		return !left.Equals(right);
	}
}
