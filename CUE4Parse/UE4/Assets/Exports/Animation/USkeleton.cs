using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class USkeleton : UObject
{
	public EBoneTranslationRetargetingMode[] BoneTree { get; private set; }

	public FReferenceSkeleton ReferenceSkeleton { get; private set; }

	public FGuid Guid { get; private set; }

	public FGuid VirtualBoneGuid { get; private set; }

	public Dictionary<FName, FReferencePose> AnimRetargetSources { get; private set; }

	public Dictionary<FName, FSmartNameMapping> NameMappings { get; private set; }

	public FName[] ExistingMarkerNames { get; private set; }

	public FPackageIndex[] Sockets { get; private set; }

	public int BoneCount => BoneTree.Length;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (TryGetValue<FStructFallback[]>(out var obj, "BoneTree"))
		{
			BoneTree = new EBoneTranslationRetargetingMode[obj.Length];
			for (int i = 0; i < BoneTree.Length; i++)
			{
				BoneTree[i] = obj[i].GetOrDefault("TranslationRetargetingMode", EBoneTranslationRetargetingMode.Animation);
			}
		}
		VirtualBoneGuid = GetOrDefault<FGuid>("VirtualBoneGuid");
		Sockets = GetOrDefault("Sockets", Array.Empty<FPackageIndex>());
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.REFERENCE_SKELETON_REFACTOR)
		{
			ReferenceSkeleton = new FReferenceSkeleton(Ar);
		}
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.FIX_ANIMATIONBASEPOSE_SERIALIZATION)
		{
			int num = Ar.Read<int>();
			AnimRetargetSources = new Dictionary<FName, FReferencePose>(num);
			for (int j = 0; j < num; j++)
			{
				FName key = Ar.ReadFName();
				FReferencePose value = new FReferencePose(Ar);
				ReferenceSkeleton.AdjustBoneScales(value.ReferencePose);
				AnimRetargetSources[key] = value;
			}
		}
		else
		{
			Log.Warning("");
		}
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.SKELETON_GUID_SERIALIZATION)
		{
			Guid = Ar.Read<FGuid>();
		}
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.SKELETON_ADD_SMARTNAMES)
		{
			int num2 = Ar.Read<int>();
			NameMappings = new Dictionary<FName, FSmartNameMapping>(num2);
			for (int k = 0; k < num2; k++)
			{
				NameMappings[Ar.ReadFName()] = new FSmartNameMapping(Ar);
			}
		}
		if (FAnimObjectVersion.Get(Ar) >= FAnimObjectVersion.Type.StoreMarkerNamesOnSkeleton && !Ar.Read<FStripDataFlags>().IsEditorDataStripped())
		{
			ExistingMarkerNames = Ar.ReadArray(Ar.ReadFName);
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("ReferenceSkeleton");
		serializer.Serialize(writer, ReferenceSkeleton);
		writer.WritePropertyName("Guid");
		serializer.Serialize(writer, Guid);
		writer.WritePropertyName("AnimRetargetSources");
		serializer.Serialize(writer, AnimRetargetSources);
		writer.WritePropertyName("NameMappings");
		serializer.Serialize(writer, NameMappings);
		writer.WritePropertyName("ExistingMarkerNames");
		serializer.Serialize(writer, ExistingMarkerNames);
	}
}
