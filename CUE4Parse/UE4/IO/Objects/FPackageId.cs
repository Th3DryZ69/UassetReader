#define TRACE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using CUE4Parse.Utils;

namespace CUE4Parse.UE4.IO.Objects;

public readonly struct FPackageId : IEquatable<FPackageId>
{
	public readonly ulong id;

	public FPackageId(ulong id)
	{
		this.id = id;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(FPackageId other)
	{
		return id == other.id;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object? obj)
	{
		if (obj is FPackageId other)
		{
			return Equals(other);
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode()
	{
		return id.GetHashCode();
	}

	public override string ToString()
	{
		return id.ToString();
	}

	public static FPackageId FromName(string name)
	{
		string text = name.ToLowerInvariant();
		ulong num = CityHash.CityHash64(Encoding.Unicode.GetBytes(text));
		Trace.Assert(num != ulong.MaxValue, "Package name hash collision \"" + text + "\" and InvalidId");
		return new FPackageId(num);
	}
}
