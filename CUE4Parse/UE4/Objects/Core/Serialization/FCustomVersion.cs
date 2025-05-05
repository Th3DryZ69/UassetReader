using System.Runtime.InteropServices;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.Objects.Core.Serialization;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FCustomVersion
{
	public FGuid Key;

	public int Version;

	public static bool operator ==(FCustomVersion one, FCustomVersion two)
	{
		if (one.Key == two.Key)
		{
			return one.Version == two.Version;
		}
		return false;
	}

	public static bool operator !=(FCustomVersion one, FCustomVersion two)
	{
		if (!(one.Key != two.Key))
		{
			return one.Version != two.Version;
		}
		return true;
	}

	public override string ToString()
	{
		return $"{"Key"}: {Key}, {"Version"}: {Version}";
	}

	public FCustomVersion(FGuid key, int version)
	{
		Key = key;
		Version = version;
	}
}
