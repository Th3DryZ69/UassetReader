using System;
using CUE4Parse.UE4.Assets;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Assets.Utils;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using CUE4Parse.Utils;
using Ionic.Crc;
using Serilog;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

[StructFallback]
public class FActorTemplateRecord
{
	public ulong ID;

	public FSoftObjectPath ActorClass;

	public FActorComponentRecord[] ActorComponents;

	public byte[]? ActorData;

	public uint DataHash;

	public short LevelRecordSaveVersion;

	public bool bUsingRecordDataReferenceTable;

	public FActorTemplateRecord(FLevelSaveRecordArchive Ar)
	{
		ID = Ar.Read<ulong>();
		if (Ar.Version < ELevelSaveRecordVersion.SoftActorClassReferences)
		{
			UObject uObject = Ar.ReadUObject();
			if (uObject != null)
			{
				string pathName = uObject.GetPathName();
				ActorClass = new FSoftObjectPath(pathName.SubstringBeforeLast(':'), "");
			}
			else
			{
				ActorClass = default(FSoftObjectPath);
			}
		}
		else
		{
			ActorClass = new FSoftObjectPath(Ar);
		}
		ActorComponents = Ar.ReadArray(() => new FActorComponentRecord(Ar));
		ActorData = Ar.ReadArray<byte>();
		CRC32 cRC = new CRC32();
		cRC.SlurpBlock(ActorData, 0, ActorData.Length);
		uint crc32Result = (uint)cRC.Crc32Result;
		if (Ar.Version < ELevelSaveRecordVersion.AddingDataHash)
		{
			DataHash = crc32Result;
			return;
		}
		DataHash = Ar.Read<uint>();
		if (DataHash != crc32Result)
		{
			Log.Error("FActorTemplateRecord::Serialize failed to deserialize data for: {0} dropping corrupted data.", ActorClass.ToString());
			ActorData = null;
			DataHash = 0u;
		}
	}

	public FActorTemplateRecord(FStructFallback fallback)
	{
		ActorClass = fallback.GetOrDefault<FSoftObjectPath>("ActorClass");
		ID = fallback.GetOrDefault("ID", 0uL);
		ActorComponents = fallback.GetOrDefault<FActorComponentRecord[]>("ActorComponents");
		ActorData = fallback.GetOrDefault<byte[]>("ActorData");
		DataHash = fallback.GetOrDefault("DataHash", 0u);
		LevelRecordSaveVersion = fallback.GetOrDefault("LevelRecordSaveVersion", (short)0, StringComparison.Ordinal);
		bUsingRecordDataReferenceTable = fallback.GetOrDefault("bUsingRecordDataReferenceTable", defaultValue: false);
	}

	public FStructFallback ReadActorData(IPackage owner, ELevelSaveRecordVersion SaveVersion)
	{
		if (ActorData != null && !bUsingRecordDataReferenceTable)
		{
			FLevelSaveRecordArchive ar = new FLevelSaveRecordArchive(new FAssetArchive(new FByteArchive("ActorData Reader", ActorData), owner), SaveVersion);
			EPackageFlags packageFlags = owner.Summary.PackageFlags;
			owner.Summary.PackageFlags &= ~EPackageFlags.PKG_UnversionedProperties;
			FStructFallback result = new FStructFallback(ar);
			owner.Summary.PackageFlags = packageFlags;
			return result;
		}
		return new FStructFallback();
	}
}
