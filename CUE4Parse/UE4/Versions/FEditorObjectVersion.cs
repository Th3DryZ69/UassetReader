using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FEditorObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		GatheredTextProcessVersionFlagging = 1,
		GatheredTextPackageCacheFixesV1 = 2,
		RootMetaDataSupport = 3,
		GatheredTextPackageCacheFixesV2 = 4,
		TextFormatArgumentDataIsVariant = 5,
		SplineComponentCurvesInStruct = 6,
		ComboBoxControllerSupportUpdate = 7,
		RefactorMeshEditorMaterials = 8,
		AddedFontFaceAssets = 9,
		UPropertryForMeshSection = 10,
		WidgetGraphSchema = 11,
		AddedBackgroundBlurContentSlot = 12,
		StableUserDefinedEnumDisplayNames = 13,
		AddedInlineFontFaceAssets = 14,
		UPropertryForMeshSectionSerialize = 15,
		FastWidgetTemplates = 16,
		MaterialThumbnailRenderingChanges = 17,
		NewSlateClippingSystem = 18,
		MovieSceneMetaDataSerialization = 19,
		GatheredTextEditorOnlyPackageLocId = 20,
		AddedAlwaysSignNumberFormattingOption = 21,
		AddedMaterialSharedInputs = 22,
		AddedMorphTargetSectionIndices = 23,
		SerializeInstancedStaticMeshRenderData = 24,
		MeshDescriptionNewSerialization_MovedToRelease = 25,
		MeshDescriptionNewAttributeFormat = 26,
		ChangeSceneCaptureRootComponent = 27,
		StaticMeshDeprecatedRawMesh = 28,
		MeshDescriptionBulkDataGuid = 29,
		MeshDescriptionRemovedHoles = 30,
		ChangedWidgetComponentWindowVisibilityDefault = 31,
		CultureInvariantTextSerializationKeyStability = 32,
		ScrollBarThicknessChange = 33,
		RemoveLandscapeHoleMaterial = 34,
		MeshDescriptionTriangles = 35,
		ComputeWeightedNormals = 36,
		SkeletalMeshBuildRefactor = 37,
		SkeletalMeshMoveEditorSourceDataToPrivateAsset = 38,
		NumberParsingOptionsNumberLimitsAndClamping = 39,
		SkeletalMeshSourceDataSupport16bitOfMaterialNumber = 40,
		VersionPlusOne = 41,
		LatestVersion = 40
	}

	public static readonly FGuid GUID = new FGuid(3836766445u, 4103357161u, 2721176075u, 776387393u);

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
			if (game < EGame.GAME_UE4_14)
			{
				if (game >= EGame.GAME_UE4_12)
				{
					if (game < EGame.GAME_UE4_13)
					{
						return Type.GatheredTextPackageCacheFixesV1;
					}
					return Type.SplineComponentCurvesInStruct;
				}
				return Type.BeforeCustomVersionWasAdded;
			}
			if (game < EGame.GAME_UE4_16)
			{
				if (game < EGame.GAME_UE4_15)
				{
					if (game == EGame.GAME_TEKKEN7)
					{
						return Type.ComboBoxControllerSupportUpdate;
					}
					return Type.RefactorMeshEditorMaterials;
				}
				return Type.AddedInlineFontFaceAssets;
			}
			if (game < EGame.GAME_UE4_17)
			{
				return Type.MaterialThumbnailRenderingChanges;
			}
			return Type.GatheredTextEditorOnlyPackageLocId;
		}
		if (game < EGame.GAME_UE4_23)
		{
			if (game < EGame.GAME_UE4_21)
			{
				if (game < EGame.GAME_UE4_20)
				{
					if (game == EGame.GAME_Paragon)
					{
						return Type.AddedMaterialSharedInputs;
					}
					return Type.AddedMorphTargetSectionIndices;
				}
				return Type.SerializeInstancedStaticMeshRenderData;
			}
			if (game < EGame.GAME_UE4_22)
			{
				return Type.MeshDescriptionNewAttributeFormat;
			}
			return Type.MeshDescriptionRemovedHoles;
		}
		if (game < EGame.GAME_UE4_25)
		{
			if (game < EGame.GAME_UE4_24)
			{
				return Type.RemoveLandscapeHoleMaterial;
			}
			return Type.SkeletalMeshBuildRefactor;
		}
		if (game < EGame.GAME_UE4_26)
		{
			return Type.SkeletalMeshMoveEditorSourceDataToPrivateAsset;
		}
		return Type.SkeletalMeshSourceDataSupport16bitOfMaterialNumber;
	}
}
