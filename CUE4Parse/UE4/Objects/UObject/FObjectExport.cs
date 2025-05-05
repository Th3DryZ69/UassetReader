using System;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Objects.UObject;

public class FObjectExport : FObjectResource
{
	public FPackageIndex ClassIndex;

	public FPackageIndex SuperIndex;

	public FPackageIndex TemplateIndex;

	public uint ObjectFlags;

	public long SerialSize;

	public long SerialOffset;

	public bool ForcedExport;

	public bool NotForClient;

	public bool NotForServer;

	public FGuid PackageGuid;

	public bool IsInheritedInstance;

	public uint PackageFlags;

	public bool NotAlwaysLoadedForEditorGame;

	public bool IsAsset;

	public bool GeneratePublicHash;

	public int FirstExportDependency;

	public int SerializationBeforeSerializationDependencies;

	public int CreateBeforeSerializationDependencies;

	public int SerializationBeforeCreateDependencies;

	public int CreateBeforeCreateDependencies;

	public Lazy<CUE4Parse.UE4.Assets.Exports.UObject> ExportObject;

	public string ClassName;

	public FObjectExport()
	{
	}

	public FObjectExport(FAssetArchive Ar)
	{
		ClassIndex = new FPackageIndex(Ar);
		SuperIndex = new FPackageIndex(Ar);
		TemplateIndex = ((Ar.Ver >= EUnrealEngineObjectUE4Version.TemplateIndex_IN_COOKED_EXPORTS) ? new FPackageIndex(Ar) : new FPackageIndex());
		OuterIndex = new FPackageIndex(Ar);
		ObjectName = Ar.ReadFName();
		ObjectFlags = Ar.Read<uint>();
		if (Ar.Ver < EUnrealEngineObjectUE4Version.e64BIT_EXPORTMAP_SERIALSIZES)
		{
			SerialSize = Ar.Read<int>();
			SerialOffset = Ar.Read<int>();
		}
		else
		{
			SerialSize = Ar.Read<long>();
			SerialOffset = Ar.Read<long>();
		}
		ForcedExport = Ar.ReadBoolean();
		NotForClient = Ar.ReadBoolean();
		NotForServer = Ar.ReadBoolean();
		PackageGuid = ((Ar.Ver < EUnrealEngineObjectUE5Version.REMOVE_OBJECT_EXPORT_PACKAGE_GUID) ? Ar.Read<FGuid>() : default(FGuid));
		IsInheritedInstance = Ar.Ver >= EUnrealEngineObjectUE5Version.TRACK_OBJECT_EXPORT_IS_INHERITED && Ar.ReadBoolean();
		PackageFlags = Ar.Read<uint>();
		NotAlwaysLoadedForEditorGame = Ar.Ver >= EUnrealEngineObjectUE4Version.LOAD_FOR_EDITOR_GAME && Ar.ReadBoolean();
		IsAsset = Ar.Ver >= EUnrealEngineObjectUE4Version.COOKED_ASSETS_IN_EDITOR_SUPPORT && Ar.ReadBoolean();
		GeneratePublicHash = Ar.Ver >= EUnrealEngineObjectUE5Version.OPTIONAL_RESOURCES && Ar.ReadBoolean();
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.PRELOAD_DEPENDENCIES_IN_COOKED_EXPORTS)
		{
			FirstExportDependency = Ar.Read<int>();
			SerializationBeforeSerializationDependencies = Ar.Read<int>();
			CreateBeforeSerializationDependencies = Ar.Read<int>();
			SerializationBeforeCreateDependencies = Ar.Read<int>();
			CreateBeforeCreateDependencies = Ar.Read<int>();
		}
		else
		{
			FirstExportDependency = -1;
			SerializationBeforeSerializationDependencies = 0;
			CreateBeforeSerializationDependencies = 0;
			SerializationBeforeCreateDependencies = 0;
			CreateBeforeCreateDependencies = 0;
		}
		ClassName = ClassIndex.Name;
	}

	public override string ToString()
	{
		return ObjectName.Text + " (" + ClassIndex.Name + ")";
	}
}
