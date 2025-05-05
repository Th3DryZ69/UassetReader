using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.Engine;

public struct FStripDataFlags
{
	private static readonly FPackageFileVersion OldestLoadablePackageFileUEVersion = FPackageFileVersion.CreateUE4Version(EUnrealEngineObjectUE4Version.OLDEST_LOADABLE_PACKAGE);

	public readonly byte GlobalStripFlags;

	public readonly byte ClassStripFlags;

	public FStripDataFlags(FArchive Ar)
		: this(Ar, in OldestLoadablePackageFileUEVersion)
	{
	}

	public FStripDataFlags(FArchive Ar, in FPackageFileVersion minVersion)
	{
		if (Ar.Ver.IsCompatible(minVersion))
		{
			GlobalStripFlags = Ar.Read<byte>();
			ClassStripFlags = Ar.Read<byte>();
		}
		else
		{
			GlobalStripFlags = (ClassStripFlags = 0);
		}
	}

	public bool IsEditorDataStripped()
	{
		return (GlobalStripFlags & 1) != 0;
	}

	public bool IsDataStrippedForServer()
	{
		return (GlobalStripFlags & 2) != 0;
	}

	public bool IsClassDataStripped(byte flag)
	{
		return (ClassStripFlags & flag) != 0;
	}
}
