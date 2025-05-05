using System.Runtime.InteropServices;
using CUE4Parse.UE4.Objects.Core.Misc;

namespace CUE4Parse.UE4.Objects.Core.Serialization;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FEnumCustomVersion_DEPRECATED
{
	public uint Tag;

	public int Version;

	public FCustomVersion ToCustomVersion()
	{
		return new FCustomVersion(new FGuid(0u, 0u, 0u, Tag), Version);
	}
}
