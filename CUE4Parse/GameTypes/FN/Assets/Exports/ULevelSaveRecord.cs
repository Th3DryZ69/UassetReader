using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.GameTypes.FN.Assets.Exports;

public class ULevelSaveRecord : UObject
{
	public FName PackageName;

	public ELevelSaveRecordVersion SaveVersion;

	public bool bCompressed;

	public FVector Center;

	public FVector HalfBoundsExtent;

	public FRotator? Rotation;

	public FVector Scale;

	public ulong LastTemplateID;

	public Dictionary<int, FActorTemplateRecord> TemplateRecords;

	public Dictionary<FGuid, FActorInstanceRecord> ActorInstanceRecords;

	public Dictionary<FGuid, FActorInstanceRecord> VolumeInfoActorRecords;

	public Dictionary<FName, FLevelStreamedDeleteActorRecord> LevelStreamedActorsToDelete;

	public int PlayerPersistenceUserWipeNumber;

	public FFortCreativeVkPalette VkPalette;

	public string IslandTemplateId;

	public byte NavmeshRequired;

	public bool bRequiresGridPlacement;

	public List<FStructFallback> ActorData;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		DeserializeHeader(Ar);
		if (SaveVersion < ELevelSaveRecordVersion.SwitchingToCoreSerialization)
		{
			FLevelSaveRecordArchive ar = new FLevelSaveRecordArchive(Ar, SaveVersion);
			DeserializeLevelSaveRecordData(ar);
			return;
		}
		if (SaveVersion >= ELevelSaveRecordVersion.AddedLevelInstance)
		{
			Ar.Position++;
		}
		base.Deserialize(Ar, validPos);
		ActorData = new List<FStructFallback>();
		foreach (KeyValuePair<FPropertyTagType, FPropertyTagType> property in GetOrDefault<UScriptMap>("TemplateRecords").Properties)
		{
			if (property.Value?.GetValue(typeof(FActorTemplateRecord)) is FActorTemplateRecord fActorTemplateRecord)
			{
				ActorData.Add(fActorTemplateRecord.ReadActorData(base.Owner, SaveVersion));
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		if (!PackageName.IsNone)
		{
			writer.WritePropertyName("PackageName");
			serializer.Serialize(writer, PackageName);
		}
		writer.WritePropertyName("SaveVersion");
		writer.WriteValue((short)SaveVersion);
		writer.WritePropertyName("bCompressed");
		writer.WriteValue(bCompressed);
		string islandTemplateId = IslandTemplateId;
		if (islandTemplateId != null && islandTemplateId.Length > 0)
		{
			writer.WritePropertyName("IslandTemplateId");
			writer.WriteValue(IslandTemplateId);
		}
		if (NavmeshRequired != 0)
		{
			writer.WritePropertyName("NavmeshRequired");
			writer.WriteValue(NavmeshRequired);
		}
		base.WriteJson(writer, serializer);
		if (Center != FVector.ZeroVector)
		{
			writer.WritePropertyName("Center");
			serializer.Serialize(writer, Center);
		}
		if (HalfBoundsExtent != FVector.ZeroVector)
		{
			writer.WritePropertyName("HalfBoundsExtent");
			serializer.Serialize(writer, HalfBoundsExtent);
		}
		if (Rotation != FRotator.ZeroRotator)
		{
			writer.WritePropertyName("Rotation");
			serializer.Serialize(writer, Rotation);
		}
		if (LastTemplateID != 0L)
		{
			writer.WritePropertyName("LastTemplateID");
			writer.WriteValue(LastTemplateID);
		}
		Dictionary<int, FActorTemplateRecord> templateRecords = TemplateRecords;
		if (templateRecords != null && templateRecords.Count > 0)
		{
			writer.WritePropertyName("TemplateRecords");
			serializer.Serialize(writer, TemplateRecords);
		}
		List<FStructFallback> actorData = ActorData;
		if (actorData != null && actorData.Count > 0)
		{
			writer.WritePropertyName("ActorData");
			serializer.Serialize(writer, ActorData);
		}
		Dictionary<FGuid, FActorInstanceRecord> actorInstanceRecords = ActorInstanceRecords;
		if (actorInstanceRecords != null && actorInstanceRecords.Count > 0)
		{
			writer.WritePropertyName("ActorInstanceRecords");
			serializer.Serialize(writer, ActorInstanceRecords);
		}
		actorInstanceRecords = VolumeInfoActorRecords;
		if (actorInstanceRecords != null && actorInstanceRecords.Count > 0)
		{
			writer.WritePropertyName("VolumeInfoActorRecords");
			serializer.Serialize(writer, VolumeInfoActorRecords);
		}
		if (bRequiresGridPlacement)
		{
			writer.WritePropertyName("bRequiresGridPlacement");
			writer.WriteValue(bRequiresGridPlacement);
		}
		if (Scale != FVector.OneVector)
		{
			writer.WritePropertyName("Scale");
			serializer.Serialize(writer, Scale);
		}
		Dictionary<FName, FLevelStreamedDeleteActorRecord> levelStreamedActorsToDelete = LevelStreamedActorsToDelete;
		if (levelStreamedActorsToDelete != null && levelStreamedActorsToDelete.Count > 0)
		{
			writer.WritePropertyName("LevelStreamedActorsToDelete");
			serializer.Serialize(writer, LevelStreamedActorsToDelete);
		}
		if (PlayerPersistenceUserWipeNumber != 0)
		{
			writer.WritePropertyName("PlayerPersistenceUserWipeNumber");
			writer.WriteValue(PlayerPersistenceUserWipeNumber);
		}
	}

	public void ReadFromArchive(FArchive Ar)
	{
		DeserializeHeader(Ar);
		byte[] array = Ar.ReadArray<byte>();
		byte[] data = (bCompressed ? ((FArchive)new FArchiveLoadCompressedProxy(Ar.Name, array, "Zlib", ECompressionFlags.COMPRESS_None, Ar.Versions)) : ((FArchive)new FByteArchive(Ar.Name, array, Ar.Versions))).ReadArray<byte>();
		FLevelSaveRecordArchive ar = new FLevelSaveRecordArchive(new FByteArchive(Ar.Name, data, Ar.Versions), SaveVersion);
		if (SaveVersion >= ELevelSaveRecordVersion.SwitchingToCoreSerialization)
		{
			throw new NotImplementedException();
		}
		DeserializeLevelSaveRecordData(ar);
	}

	private void DeserializeHeader(FArchive Ar)
	{
		PackageName = Ar.ReadFName();
		SaveVersion = Ar.Read<ELevelSaveRecordVersion>();
		if (SaveVersion > ELevelSaveRecordVersion.RemoveInvalidEventBindings)
		{
			short saveVersion = (short)SaveVersion;
			Log.Warning("Unsupported level save record version " + saveVersion);
		}
		bCompressed = Ar.ReadBoolean();
		if (SaveVersion >= ELevelSaveRecordVersion.AddedIslandTemplateId)
		{
			IslandTemplateId = Ar.ReadFString();
		}
		if (SaveVersion >= ELevelSaveRecordVersion.AddedNavmeshRequired)
		{
			NavmeshRequired = Ar.Read<byte>();
		}
	}

	private void DeserializeLevelSaveRecordData(FLevelSaveRecordArchive Ar)
	{
		Center = new FVector(Ar);
		HalfBoundsExtent = new FVector(Ar);
		Rotation = new FRotator(Ar);
		LastTemplateID = Ar.Read<ulong>();
		int num = Ar.Read<int>();
		TemplateRecords = new Dictionary<int, FActorTemplateRecord>(num);
		for (int i = 0; i < num; i++)
		{
			TemplateRecords[Ar.Read<int>()] = new FActorTemplateRecord(Ar);
		}
		int num2 = Ar.Read<int>();
		ActorInstanceRecords = new Dictionary<FGuid, FActorInstanceRecord>(num2);
		for (int j = 0; j < num2; j++)
		{
			ActorInstanceRecords[Ar.Read<FGuid>()] = new FActorInstanceRecord(Ar);
		}
		int num3 = Ar.Read<int>();
		VolumeInfoActorRecords = new Dictionary<FGuid, FActorInstanceRecord>(num3);
		for (int k = 0; k < num3; k++)
		{
			VolumeInfoActorRecords[Ar.Read<FGuid>()] = new FActorInstanceRecord(Ar);
		}
		bRequiresGridPlacement = Ar.ReadBoolean();
		Scale = ((Ar.Version >= ELevelSaveRecordVersion.AddingScale) ? new FVector(Ar) : FVector.OneVector);
		if (Ar.Version >= ELevelSaveRecordVersion.AddedLevelStreamedDeleteRecord)
		{
			int num4 = Ar.Read<int>();
			LevelStreamedActorsToDelete = new Dictionary<FName, FLevelStreamedDeleteActorRecord>();
			for (int l = 0; l < num4; l++)
			{
				LevelStreamedActorsToDelete[Ar.ReadFName()] = new FLevelStreamedDeleteActorRecord(Ar);
			}
		}
		if (Ar.Version >= ELevelSaveRecordVersion.AddedPlayerPersistenceUserWipeNumber)
		{
			PlayerPersistenceUserWipeNumber = Ar.Read<int>();
		}
		if (Ar.Version >= ELevelSaveRecordVersion.AddedVkPalette)
		{
			VkPalette = new FFortCreativeVkPalette(Ar);
		}
	}
}
