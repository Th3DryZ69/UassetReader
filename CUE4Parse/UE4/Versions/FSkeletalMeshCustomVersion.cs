using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FSkeletalMeshCustomVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		CombineSectionWithChunk = 1,
		CombineSoftAndRigidVerts = 2,
		RecalcMaxBoneInfluences = 3,
		SaveNumVertices = 4,
		RegenerateClothingShadowFlags = 5,
		UseSharedColorBufferFormat = 6,
		UseSeparateSkinWeightBuffer = 7,
		NewClothingSystemAdded = 8,
		CachedClothInverseMasses = 9,
		CompactClothVertexBuffer = 10,
		RemoveSourceData = 11,
		SplitModelAndRenderData = 12,
		RemoveTriangleSorting = 13,
		RemoveDuplicatedClothingSections = 14,
		DeprecateSectionDisabledFlag = 15,
		SectionIgnoreByReduceAdded = 16,
		SkinWeightProfiles = 17,
		RemoveEnableClothLOD = 18,
		VersionPlusOne = 19,
		LatestVersion = 18
	}

	public static readonly FGuid GUID = new FGuid(3616164352u, 3898099351u, 3131578805u, 1216169652u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_19)
		{
			if (game < EGame.GAME_UE4_16)
			{
				if (game < EGame.GAME_UE4_14)
				{
					if (game < EGame.GAME_UE4_13)
					{
						return Type.BeforeCustomVersionWasAdded;
					}
					return Type.SaveNumVertices;
				}
				if (game < EGame.GAME_UE4_15)
				{
					return Type.RegenerateClothingShadowFlags;
				}
				return Type.UseSeparateSkinWeightBuffer;
			}
			if (game < EGame.GAME_UE4_18)
			{
				return Type.CachedClothInverseMasses;
			}
			return Type.CompactClothVertexBuffer;
		}
		if (game < EGame.GAME_UE4_23)
		{
			if (game < EGame.GAME_UE4_20)
			{
				if (game == EGame.GAME_Paragon)
				{
					return Type.SplitModelAndRenderData;
				}
				return Type.DeprecateSectionDisabledFlag;
			}
			return Type.SectionIgnoreByReduceAdded;
		}
		if (game < EGame.GAME_UE4_26)
		{
			return Type.SkinWeightProfiles;
		}
		return Type.RemoveEnableClothLOD;
	}
}
