using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.UObject;

public class FObjectImport : FObjectResource
{
	public readonly FName ClassPackage;

	public FName ClassName;

	public FName PackageName;

	public bool ImportOptional;

	public FObjectImport()
	{
	}

	public FObjectImport(FAssetArchive Ar)
	{
		ClassPackage = Ar.ReadFName();
		ClassName = Ar.ReadFName();
		OuterIndex = new FPackageIndex(Ar);
		ObjectName = Ar.ReadFName();
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.NON_OUTER_PACKAGE_IMPORT && !Ar.IsFilterEditorOnly)
		{
			PackageName = Ar.ReadFName();
		}
		ImportOptional = Ar.Ver >= EUnrealEngineObjectUE5Version.OPTIONAL_RESOURCES && Ar.ReadBoolean();
	}
}
