using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FBlueprintsObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		OverridenEventReferenceFixup = 1,
		CleanBlueprintFunctionFlags = 2,
		ArrayGetByRefUpgrade = 3,
		EdGraphPinOptimized = 4,
		AllowDeletionConformed = 5,
		AdvancedContainerSupport = 6,
		SCSHasComponentTemplateClass = 7,
		ComponentTemplateClassSupport = 8,
		ArrayGetFuncsReplacedByCustomNode = 9,
		DisallowObjectConfigVars = 10,
		VersionPlusOne = 11,
		LatestVersion = 10
	}

	public static readonly FGuid GUID = new FGuid(2966958820u, 529092365u, 2899279543u, 922569378u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		_ = Ar.Game;
		return Type.DisallowObjectConfigVars;
	}
}
