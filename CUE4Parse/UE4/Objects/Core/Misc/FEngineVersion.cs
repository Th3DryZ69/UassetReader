using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Objects.Core.Misc;

public class FEngineVersion : FEngineVersionBase
{
	private string _branch;

	public string Branch => _branch.Replace('+', '/');

	public FEngineVersion(FArchive Ar)
		: base(Ar)
	{
		_branch = Ar.ReadFString();
	}

	public FEngineVersion(ushort major, ushort minor, ushort patch, uint changelist, string branch)
		: base(major, minor, patch, changelist)
	{
		_branch = branch.Replace('/', '+');
	}

	public void Set(ushort major, ushort minor, ushort patch, uint changelist, string branch)
	{
		Major = major;
		Minor = minor;
		Patch = patch;
		_changelist = changelist;
		_branch = branch.Replace('/', '+');
	}

	public string ToString(EVersionComponent lastComponent)
	{
		string text = Major.ToString();
		if (lastComponent >= EVersionComponent.Minor)
		{
			text = text + "." + Minor;
			if (lastComponent >= EVersionComponent.Patch)
			{
				text = text + "." + Patch;
				if (lastComponent >= EVersionComponent.Changelist)
				{
					text = text + "-" + base.Changelist;
					if (lastComponent >= EVersionComponent.Branch && _branch.Length > 0)
					{
						text = text + "+" + _branch;
					}
				}
			}
		}
		return text;
	}

	public override string ToString()
	{
		return ToString(EVersionComponent.Branch);
	}
}
