using System;
using System.Runtime.CompilerServices;

namespace CUE4Parse.UE4.Versions;

public struct FPackageFileVersion : IComparable<EUnrealEngineObjectUE4Version>, IComparable<EUnrealEngineObjectUE5Version>
{
	public int FileVersionUE4;

	public int FileVersionUE5;

	public int Value
	{
		get
		{
			if (FileVersionUE5 < 1000)
			{
				return FileVersionUE4;
			}
			return FileVersionUE5;
		}
		set
		{
			if (value >= 1000)
			{
				FileVersionUE5 = value;
			}
			else
			{
				FileVersionUE4 = value;
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Reset()
	{
		FileVersionUE4 = 0;
		FileVersionUE5 = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FPackageFileVersion(int ue4Version, int ue5Version)
	{
		FileVersionUE4 = ue4Version;
		FileVersionUE5 = ue5Version;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FPackageFileVersion CreateUE4Version(int version)
	{
		return new FPackageFileVersion(version, 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FPackageFileVersion CreateUE4Version(EUnrealEngineObjectUE4Version version)
	{
		return new FPackageFileVersion((int)version, 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FPackageFileVersion a, EUnrealEngineObjectUE4Version b)
	{
		return a.FileVersionUE4 == (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FPackageFileVersion a, EUnrealEngineObjectUE4Version b)
	{
		return a.FileVersionUE4 != (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <(FPackageFileVersion a, EUnrealEngineObjectUE4Version b)
	{
		return a.FileVersionUE4 < (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >(FPackageFileVersion a, EUnrealEngineObjectUE4Version b)
	{
		return a.FileVersionUE4 > (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <=(FPackageFileVersion a, EUnrealEngineObjectUE4Version b)
	{
		return a.FileVersionUE4 <= (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >=(FPackageFileVersion a, EUnrealEngineObjectUE4Version b)
	{
		return a.FileVersionUE4 >= (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(EUnrealEngineObjectUE4Version other)
	{
		return FileVersionUE4.CompareTo(other);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FPackageFileVersion a, EUnrealEngineObjectUE5Version b)
	{
		return a.FileVersionUE5 == (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FPackageFileVersion a, EUnrealEngineObjectUE5Version b)
	{
		return a.FileVersionUE5 != (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <(FPackageFileVersion a, EUnrealEngineObjectUE5Version b)
	{
		return a.FileVersionUE5 < (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >(FPackageFileVersion a, EUnrealEngineObjectUE5Version b)
	{
		return a.FileVersionUE5 > (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator <=(FPackageFileVersion a, EUnrealEngineObjectUE5Version b)
	{
		return a.FileVersionUE5 <= (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator >=(FPackageFileVersion a, EUnrealEngineObjectUE5Version b)
	{
		return a.FileVersionUE5 >= (int)b;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int CompareTo(EUnrealEngineObjectUE5Version other)
	{
		return FileVersionUE5.CompareTo(other);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsCompatible(FPackageFileVersion other)
	{
		if (FileVersionUE4 >= other.FileVersionUE4)
		{
			return FileVersionUE5 >= other.FileVersionUE5;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(FPackageFileVersion a, FPackageFileVersion b)
	{
		if (a.FileVersionUE4 == b.FileVersionUE4)
		{
			return a.FileVersionUE5 == b.FileVersionUE5;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(FPackageFileVersion a, FPackageFileVersion b)
	{
		return !(a == b);
	}

	public override bool Equals(object? obj)
	{
		if (obj is FPackageFileVersion fPackageFileVersion)
		{
			return this == fPackageFileVersion;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(FileVersionUE4, FileVersionUE5);
	}

	public override string ToString()
	{
		if (FileVersionUE5 < 1000)
		{
			EUnrealEngineObjectUE4Version fileVersionUE = (EUnrealEngineObjectUE4Version)FileVersionUE4;
			return fileVersionUE.ToString();
		}
		EUnrealEngineObjectUE5Version fileVersionUE2 = (EUnrealEngineObjectUE5Version)FileVersionUE5;
		return fileVersionUE2.ToString();
	}
}
