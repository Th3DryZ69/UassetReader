using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FFrameworkObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		UseBodySetupCollisionProfile = 1,
		AnimBlueprintSubgraphFix = 2,
		MeshSocketScaleUtilization = 3,
		ExplicitAttachmentRules = 4,
		MoveCompressedAnimDataToTheDDC = 5,
		FixNonTransactionalPins = 6,
		SmartNameRefactor = 7,
		AddSourceReferenceSkeletonToRig = 8,
		ConstraintInstanceBehaviorParameters = 9,
		PoseAssetSupportPerBoneMask = 10,
		PhysAssetUseSkeletalBodySetup = 11,
		RemoveSoundWaveCompressionName = 12,
		AddInternalClothingGraphicalSkinning = 13,
		WheelOffsetIsFromWheel = 14,
		MoveCurveTypesToSkeleton = 15,
		CacheDestructibleOverlaps = 16,
		GeometryCacheMissingMaterials = 17,
		LODsUseResolutionIndependentScreenSize = 18,
		BlendSpacePostLoadSnapToGrid = 19,
		SupportBlendSpaceRateScale = 20,
		LODHysteresisUseResolutionIndependentScreenSize = 21,
		ChangeAudioComponentOverrideSubtitlePriorityDefault = 22,
		HardSoundReferences = 23,
		EnforceConstInAnimBlueprintFunctionGraphs = 24,
		InputKeySelectorTextStyle = 25,
		EdGraphPinContainerType = 26,
		ChangeAssetPinsToString = 27,
		LocalVariablesBlueprintVisible = 28,
		RemoveUField_Next = 29,
		UserDefinedStructsBlueprintVisible = 30,
		PinsStoreFName = 31,
		UserDefinedStructsStoreDefaultInstance = 32,
		FunctionTerminatorNodesUseMemberReference = 33,
		EditableEventsUseConstRefParameters = 34,
		BlueprintGeneratedClassIsAlwaysAuthoritative = 35,
		EnforceBlueprintFunctionVisibility = 36,
		StoringUCSSerializationIndex = 37,
		VersionPlusOne = 38,
		LatestVersion = 37
	}

	public static readonly FGuid GUID = new FGuid(3489428543u, 1135625344u, 2475758815u, 387784819u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_17)
		{
			if (game < EGame.GAME_UE4_15)
			{
				if (game < EGame.GAME_UE4_13)
				{
					if (game < EGame.GAME_UE4_12)
					{
						return Type.BeforeCustomVersionWasAdded;
					}
					return Type.FixNonTransactionalPins;
				}
				if (game >= EGame.GAME_UE4_14)
				{
					if (game == EGame.GAME_TEKKEN7)
					{
						return Type.WheelOffsetIsFromWheel;
					}
					return Type.GeometryCacheMissingMaterials;
				}
				return Type.RemoveSoundWaveCompressionName;
			}
			if (game < EGame.GAME_UE4_16)
			{
				return Type.ChangeAudioComponentOverrideSubtitlePriorityDefault;
			}
			return Type.HardSoundReferences;
		}
		if (game < EGame.GAME_UE4_22)
		{
			if (game < EGame.GAME_UE4_19)
			{
				if (game < EGame.GAME_UE4_18)
				{
					return Type.LocalVariablesBlueprintVisible;
				}
				return Type.UserDefinedStructsBlueprintVisible;
			}
			if (game < EGame.GAME_UE4_20)
			{
				return Type.FunctionTerminatorNodesUseMemberReference;
			}
			return Type.EditableEventsUseConstRefParameters;
		}
		if (game < EGame.GAME_UE4_25)
		{
			if (game < EGame.GAME_UE4_24)
			{
				return Type.BlueprintGeneratedClassIsAlwaysAuthoritative;
			}
			return Type.EnforceBlueprintFunctionVisibility;
		}
		if (game < EGame.GAME_UE4_26)
		{
			return Type.StoringUCSSerializationIndex;
		}
		return Type.StoringUCSSerializationIndex;
	}
}
