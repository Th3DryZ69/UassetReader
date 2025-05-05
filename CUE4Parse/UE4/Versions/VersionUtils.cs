using System.Runtime.CompilerServices;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Core.Serialization;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class VersionUtils
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int CustomVer(this FArchive Ar, FGuid key)
	{
		FCustomVersionContainer customVersions = Ar.Versions.CustomVersions;
		if (customVersions != null)
		{
			int version = customVersions.GetVersion(key);
			if (version != -1)
			{
				return version;
			}
		}
		FPackageFileSummary fPackageFileSummary = (Ar as FAssetArchive)?.Owner.Summary;
		if (fPackageFileSummary != null && !fPackageFileSummary.bUnversioned)
		{
			int version2 = fPackageFileSummary.CustomVersionContainer.GetVersion(key);
			if (version2 == -1)
			{
				return 0;
			}
			return version2;
		}
		return -1;
	}
}
