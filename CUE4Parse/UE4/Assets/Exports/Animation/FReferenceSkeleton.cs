using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

[JsonConverter(typeof(FReferenceSkeletonConverter))]
public class FReferenceSkeleton
{
	public readonly FMeshBoneInfo[] FinalRefBoneInfo;

	public readonly FTransform[] FinalRefBonePose;

	public readonly Dictionary<string, int> FinalNameToIndexMap;

	public FReferenceSkeleton(FAssetArchive Ar)
	{
		FinalRefBoneInfo = Ar.ReadArray(() => new FMeshBoneInfo(Ar));
		FinalRefBonePose = Ar.ReadArray(() => new FTransform(Ar));
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.REFERENCE_SKELETON_REFACTOR)
		{
			int num = Ar.Read<int>();
			FinalNameToIndexMap = new Dictionary<string, int>(num);
			for (int num2 = 0; num2 < num; num2++)
			{
				FinalNameToIndexMap[Ar.ReadFName().Text] = Ar.Read<int>();
			}
		}
		else
		{
			FinalNameToIndexMap = new Dictionary<string, int>();
		}
		if (Ar.Ver < EUnrealEngineObjectUE4Version.FIXUP_ROOTBONE_PARENT && FinalRefBoneInfo.Length != 0 && FinalRefBoneInfo[0].ParentIndex != -1)
		{
			FinalRefBoneInfo[0] = new FMeshBoneInfo(FinalRefBoneInfo[0].Name, -1);
		}
		AdjustBoneScales(FinalRefBonePose);
	}

	public void AdjustBoneScales(FTransform[] transforms)
	{
		if (FinalRefBoneInfo.Length == transforms.Length)
		{
			for (int i = 0; i < transforms.Length; i++)
			{
				FVector boneScale = GetBoneScale(transforms, i);
				transforms[i].Translation.Scale(boneScale);
			}
		}
	}

	public FVector GetBoneScale(FTransform[] transforms, int boneIndex)
	{
		FVector result = new FVector(1f);
		for (boneIndex = FinalRefBoneInfo[boneIndex].ParentIndex; boneIndex >= 0; boneIndex = FinalRefBoneInfo[boneIndex].ParentIndex)
		{
			FVector scale3D = transforms[boneIndex].Scale3D;
			result.Scale(scale3D);
		}
		return result;
	}
}
