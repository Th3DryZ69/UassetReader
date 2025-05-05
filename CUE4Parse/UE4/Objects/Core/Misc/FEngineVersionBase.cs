using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Core.Misc;

public class FEngineVersionBase
{
	public ushort Major;

	public ushort Minor;

	public ushort Patch;

	protected uint _changelist;

	public uint Changelist => _changelist & 0x7FFFFFFF;

	public FEngineVersionBase(FArchive Ar)
	{
		Major = Ar.Read<ushort>();
		Minor = Ar.Read<ushort>();
		Patch = Ar.Read<ushort>();
		_changelist = Ar.Read<uint>();
	}

	public FEngineVersionBase(ushort major, ushort minor, ushort patch, uint changelist)
	{
		Major = major;
		Minor = minor;
		Patch = patch;
		_changelist = changelist;
	}

	public bool IsLicenseeVersion()
	{
		return (_changelist & 0x80000000u) != 0;
	}

	public bool IsEmpty()
	{
		if (Major == 0 && Minor == 0)
		{
			return Patch == 0;
		}
		return false;
	}

	public bool HasChangelist()
	{
		return Changelist != 0;
	}

	public static uint EncodeLicenseeChangeList(uint changelist)
	{
		return changelist | 0x80000000u;
	}
}
