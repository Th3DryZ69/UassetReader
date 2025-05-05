using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Readers;

namespace CUE4Parse.UE4.Versions;

public static class FAnimPhysObjectVersion
{
	public enum Type
	{
		BeforeCustomVersionWasAdded = 0,
		ConvertAnimNodeLookAtAxis = 1,
		BoxSphylElemsUseRotators = 2,
		ThumbnailSceneInfoAndAssetImportDataAreTransactional = 3,
		AddedClothingMaskWorkflow = 4,
		RemoveUIDFromSmartNameSerialize = 5,
		CreateTargetReference = 6,
		TuneSoftLimitStiffnessAndDamping = 7,
		FixInvalidClothParticleMasses = 8,
		CacheClothMeshInfluences = 9,
		SmartNameRefactorForDeterministicCooking = 10,
		RenameDisableAnimCurvesToAllowAnimCurveEvaluation = 11,
		AddLODToCurveMetaData = 12,
		FixupBadBlendProfileReferences = 13,
		AllowMultipleAudioPluginSettings = 14,
		ChangeRetargetSourceReferenceToSoftObjectPtr = 15,
		SaveEditorOnlyFullPoseForPoseAsset = 16,
		GeometryCacheAssetDeprecation = 17,
		VersionPlusOne = 18,
		LatestVersion = 17
	}

	public static readonly FGuid GUID = new FGuid(702903773u, 3768796711u, 2635125366u, 590142698u);

	public static Type Get(FArchive Ar)
	{
		int num = Ar.CustomVer(GUID);
		if (num >= 0)
		{
			return (Type)num;
		}
		EGame game = Ar.Game;
		if (game < EGame.GAME_UE4_18)
		{
			if (game >= EGame.GAME_UE4_16)
			{
				if (game < EGame.GAME_UE4_17)
				{
					return Type.ThumbnailSceneInfoAndAssetImportDataAreTransactional;
				}
				return Type.TuneSoftLimitStiffnessAndDamping;
			}
			return Type.BeforeCustomVersionWasAdded;
		}
		if (game < EGame.GAME_UE4_20)
		{
			if (game < EGame.GAME_UE4_19)
			{
				return Type.AddLODToCurveMetaData;
			}
			return Type.SaveEditorOnlyFullPoseForPoseAsset;
		}
		if (game < EGame.GAME_UE4_26)
		{
			return Type.GeometryCacheAssetDeprecation;
		}
		return Type.GeometryCacheAssetDeprecation;
	}
}
