using System;
using CUE4Parse.UE4.Assets.Exports.Animation;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class USkeletalMesh : UObject
{
	public FBoxSphereBounds ImportedBounds { get; private set; }

	public FSkeletalMaterial[] SkeletalMaterials { get; private set; }

	public FReferenceSkeleton ReferenceSkeleton { get; private set; }

	public FStaticLODModel[]? LODModels { get; private set; }

	public bool bHasVertexColors { get; private set; }

	public byte NumVertexColorChannels { get; private set; }

	public FPackageIndex[] MorphTargets { get; private set; }

	public FPackageIndex[] Sockets { get; private set; }

	public FPackageIndex Skeleton { get; private set; }

	public ResolvedObject?[] Materials { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Materials = Array.Empty<ResolvedObject>();
		bHasVertexColors = GetOrDefault("bHasVertexColors", defaultValue: false);
		NumVertexColorChannels = GetOrDefault("NumVertexColorChannels", (byte)0, StringComparison.Ordinal);
		MorphTargets = GetOrDefault("MorphTargets", Array.Empty<FPackageIndex>());
		Sockets = GetOrDefault("Sockets", Array.Empty<FPackageIndex>());
		Skeleton = GetOrDefault("Skeleton", new FPackageIndex());
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		ImportedBounds = new FBoxSphereBounds(Ar);
		SkeletalMaterials = Ar.ReadArray(() => new FSkeletalMaterial(Ar));
		Materials = new ResolvedObject[SkeletalMaterials.Length];
		for (int num = 0; num < Materials.Length; num++)
		{
			Materials[num] = SkeletalMaterials[num].Material;
		}
		ReferenceSkeleton = new FReferenceSkeleton(Ar);
		if (FSkeletalMeshCustomVersion.Get(Ar) < FSkeletalMeshCustomVersion.Type.SplitModelAndRenderData)
		{
			LODModels = Ar.ReadArray(() => new FStaticLODModel(Ar, bHasVertexColors));
		}
		else
		{
			if (!fStripDataFlags.IsEditorDataStripped())
			{
				LODModels = Ar.ReadArray(() => new FStaticLODModel(Ar, bHasVertexColors));
			}
			bool num2 = Ar.ReadBoolean();
			if (Ar.Versions["SkeletalMesh.KeepMobileMinLODSettingOnDesktop"])
			{
				Ar.Read<int>();
			}
			if (num2 && LODModels == null)
			{
				bool flag = Ar.Versions["SkeletalMesh.UseNewCookedFormat"];
				LODModels = new FStaticLODModel[Ar.Read<int>()];
				for (int num3 = 0; num3 < LODModels.Length; num3++)
				{
					LODModels[num3] = new FStaticLODModel();
					if (flag)
					{
						LODModels[num3].SerializeRenderItem(Ar, bHasVertexColors, NumVertexColorChannels);
					}
					else
					{
						LODModels[num3].SerializeRenderItem_Legacy(Ar, bHasVertexColors, NumVertexColorChannels);
					}
				}
				if (flag)
				{
					Ar.Read<byte>();
					Ar.Read<byte>();
				}
			}
		}
		if (Ar.Ver < EUnrealEngineObjectUE4Version.REFERENCE_SKELETON_REFACTOR)
		{
			int num4 = Ar.Read<int>();
			Ar.Position += 12 * num4;
		}
		Ar.ReadArray(() => new FPackageIndex(Ar));
		if (!TryGetValue<FStructFallback[]>(out var obj, "LODInfo"))
		{
			return;
		}
		for (int num5 = 0; num5 < LODModels?.Length; num5++)
		{
			FStructFallback fStructFallback = ((num5 < obj.Length) ? obj[num5] : null);
			if (fStructFallback == null || !fStructFallback.TryGetValue<int[]>(out var obj2, "LODMaterialMap"))
			{
				continue;
			}
			FStaticLODModel fStaticLODModel = LODModels[num5];
			for (int num6 = 0; num6 < fStaticLODModel.Sections?.Length; num6++)
			{
				if (num6 < obj2.Length && obj2[num6] >= 0 && obj2[num6] < Materials.Length)
				{
					fStaticLODModel.Sections[num6].MaterialIndex = (short)Math.Clamp((ushort)obj2[num6], 0, Materials.Length);
				}
			}
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("ImportedBounds");
		serializer.Serialize(writer, ImportedBounds);
		writer.WritePropertyName("Materials");
		serializer.Serialize(writer, Materials);
		writer.WritePropertyName("LODModels");
		serializer.Serialize(writer, LODModels);
	}
}
