using CUE4Parse.GameTypes.FN.Objects;
using CUE4Parse.GameTypes.SWJS.Objects;
using CUE4Parse.GameTypes.TSW.Objects;
using CUE4Parse.UE4.Assets.Exports.Engine.Font;
using CUE4Parse.UE4.Assets.Exports.Material;
using CUE4Parse.UE4.Assets.Exports.SkeletalMesh;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Core.Misc;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.Engine.Ai;
using CUE4Parse.UE4.Objects.Engine.Animation;
using CUE4Parse.UE4.Objects.Engine.Curves;
using CUE4Parse.UE4.Objects.Engine.GameFramework;
using CUE4Parse.UE4.Objects.GameplayTags;
using CUE4Parse.UE4.Objects.LevelSequence;
using CUE4Parse.UE4.Objects.MovieScene;
using CUE4Parse.UE4.Objects.MovieScene.Evaluation;
using CUE4Parse.UE4.Objects.Niagara;
using CUE4Parse.UE4.Objects.UObject;
using CUE4Parse.UE4.Objects.WorldCondition;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

[JsonConverter(typeof(UScriptStructConverter))]
public class UScriptStruct
{
	public readonly IUStruct StructType;

	public UScriptStruct(FAssetArchive Ar, string? structName, UStruct? struc, ReadType? type)
	{
		IUStruct structType;
		switch (structName)
		{
		case "Box":
			structType = ((type == ReadType.ZERO) ? default(FBox) : new FBox(Ar));
			break;
		case "Box2D":
			structType = ((type == ReadType.ZERO) ? new FBox2D() : new FBox2D(Ar));
			break;
		case "Color":
			structType = ((type == ReadType.ZERO) ? default(FColor) : Ar.Read<FColor>());
			break;
		case "ColorMaterialInput":
			structType = ((type == ReadType.ZERO) ? new FMaterialInput<FColor>() : new FMaterialInput<FColor>(Ar));
			break;
		case "DateTime":
			structType = ((type == ReadType.ZERO) ? default(FDateTime) : Ar.Read<FDateTime>());
			break;
		case "ExpressionInput":
			structType = ((type == ReadType.ZERO) ? new FExpressionInput() : new FExpressionInput(Ar));
			break;
		case "FrameNumber":
			structType = ((type == ReadType.ZERO) ? default(FFrameNumber) : Ar.Read<FFrameNumber>());
			break;
		case "Guid":
			structType = ((type == ReadType.ZERO) ? default(FGuid) : Ar.Read<FGuid>());
			break;
		case "NavAgentSelector":
			structType = ((type == ReadType.ZERO) ? default(FNavAgentSelector) : Ar.Read<FNavAgentSelector>());
			break;
		case "SmartName":
			structType = ((type == ReadType.ZERO) ? default(FSmartName) : new FSmartName(Ar));
			break;
		case "RichCurveKey":
			structType = ((type == ReadType.ZERO) ? default(FRichCurveKey) : Ar.Read<FRichCurveKey>());
			break;
		case "SimpleCurveKey":
			structType = ((type == ReadType.ZERO) ? default(FSimpleCurveKey) : Ar.Read<FSimpleCurveKey>());
			break;
		case "ScalarMaterialInput":
			structType = ((type == ReadType.ZERO) ? new FMaterialInput<float>() : new FMaterialInput<float>(Ar));
			break;
		case "ShadingModelMaterialInput":
			structType = ((type == ReadType.ZERO) ? new FMaterialInput<uint>() : new FMaterialInput<uint>(Ar));
			break;
		case "VectorMaterialInput":
			structType = ((type == ReadType.ZERO) ? new FMaterialInputVector() : new FMaterialInputVector(Ar));
			break;
		case "Vector2MaterialInput":
			structType = ((type == ReadType.ZERO) ? new FMaterialInputVector2D() : new FMaterialInputVector2D(Ar));
			break;
		case "MaterialAttributesInput":
			structType = ((type == ReadType.ZERO) ? new FExpressionInput() : new FExpressionInput(Ar));
			break;
		case "SkeletalMeshSamplingLODBuiltData":
			structType = ((type == ReadType.ZERO) ? default(FSkeletalMeshSamplingLODBuiltData) : new FSkeletalMeshSamplingLODBuiltData(Ar));
			break;
		case "SkeletalMeshSamplingRegionBuiltData":
			structType = ((type == ReadType.ZERO) ? default(FSkeletalMeshSamplingRegionBuiltData) : new FSkeletalMeshSamplingRegionBuiltData(Ar));
			break;
		case "PerPlatformBool":
			structType = ((type == ReadType.ZERO) ? new TPerPlatformProperty.FPerPlatformBool() : new TPerPlatformProperty.FPerPlatformBool(Ar));
			break;
		case "PerPlatformFloat":
			structType = ((type == ReadType.ZERO) ? new TPerPlatformProperty.FPerPlatformFloat() : new TPerPlatformProperty.FPerPlatformFloat(Ar));
			break;
		case "PerPlatformInt":
			structType = ((type == ReadType.ZERO) ? new TPerPlatformProperty.FPerPlatformInt() : new TPerPlatformProperty.FPerPlatformInt(Ar));
			break;
		case "PerQualityLevelInt":
			structType = ((type == ReadType.ZERO) ? new FPerQualityLevelInt() : new FPerQualityLevelInt(Ar));
			break;
		case "GameplayTagContainer":
			structType = ((type == ReadType.ZERO) ? default(FGameplayTagContainer) : new FGameplayTagContainer(Ar));
			break;
		case "IntPoint":
			structType = ((type == ReadType.ZERO) ? default(FIntPoint) : Ar.Read<FIntPoint>());
			break;
		case "IntVector":
			structType = ((type == ReadType.ZERO) ? default(FIntVector) : Ar.Read<FIntVector>());
			break;
		case "LevelSequenceObjectReferenceMap":
			structType = ((type == ReadType.ZERO) ? default(FLevelSequenceObjectReferenceMap) : new FLevelSequenceObjectReferenceMap(Ar));
			break;
		case "LinearColor":
			structType = ((type == ReadType.ZERO) ? default(FLinearColor) : Ar.Read<FLinearColor>());
			break;
		case "NiagaraVariable":
			structType = new FNiagaraVariable(Ar);
			break;
		case "NiagaraVariableBase":
			structType = new FNiagaraVariableBase(Ar);
			break;
		case "NiagaraVariableWithOffset":
			structType = new FNiagaraVariableWithOffset(Ar);
			break;
		case "NiagaraDataInterfaceGPUParamInfo":
			structType = new FNiagaraDataInterfaceGPUParamInfo(Ar);
			break;
		case "MaterialOverrideNanite":
			structType = ((type == ReadType.ZERO) ? default(FMaterialOverrideNanite) : new FMaterialOverrideNanite(Ar));
			break;
		case "MovieSceneEvalTemplatePtr":
			structType = new FMovieSceneEvalTemplatePtr(Ar);
			break;
		case "MovieSceneEvaluationFieldEntityTree":
			structType = new FMovieSceneEvaluationFieldEntityTree(Ar);
			break;
		case "MovieSceneEvaluationKey":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneEvaluationKey) : Ar.Read<FMovieSceneEvaluationKey>());
			break;
		case "MovieSceneFloatChannel":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneChannel<float>) : new FMovieSceneChannel<float>(Ar));
			break;
		case "MovieSceneDoubleChannel":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneChannel<double>) : new FMovieSceneChannel<double>(Ar));
			break;
		case "MovieSceneFloatValue":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneValue<float>) : Ar.Read<FMovieSceneValue<float>>());
			break;
		case "MovieSceneDoubleValue":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneValue<double>) : Ar.Read<FMovieSceneValue<double>>());
			break;
		case "MovieSceneFrameRange":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneFrameRange) : Ar.Read<FMovieSceneFrameRange>());
			break;
		case "MovieSceneSegment":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneSegment) : new FMovieSceneSegment(Ar));
			break;
		case "MovieSceneSegmentIdentifier":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneSegmentIdentifier) : Ar.Read<FMovieSceneSegmentIdentifier>());
			break;
		case "MovieSceneSequenceID":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneSequenceID) : Ar.Read<FMovieSceneSequenceID>());
			break;
		case "MovieSceneTrackIdentifier":
			structType = ((type == ReadType.ZERO) ? default(FMovieSceneTrackIdentifier) : Ar.Read<FMovieSceneTrackIdentifier>());
			break;
		case "MovieSceneTrackImplementationPtr":
			structType = new FMovieSceneTrackImplementationPtr(Ar);
			break;
		case "FontData":
			structType = new FFontData(Ar);
			break;
		case "FontCharacter":
			structType = new FFontCharacter(Ar);
			break;
		case "Plane":
			structType = ((type == ReadType.ZERO) ? default(FPlane) : new FPlane(Ar));
			break;
		case "Quat":
			structType = ((type == ReadType.ZERO) ? default(FQuat) : new FQuat(Ar));
			break;
		case "Rotator":
			structType = ((type == ReadType.ZERO) ? new FRotator() : new FRotator(Ar));
			break;
		case "SectionEvaluationDataTree":
			structType = ((type == ReadType.ZERO) ? default(FSectionEvaluationDataTree) : new FSectionEvaluationDataTree(Ar));
			break;
		case "StringClassReference":
			structType = ((type == ReadType.ZERO) ? default(FSoftObjectPath) : new FSoftObjectPath(Ar));
			break;
		case "SoftClassPath":
			structType = ((type == ReadType.ZERO) ? default(FSoftObjectPath) : new FSoftObjectPath(Ar));
			break;
		case "StringAssetReference":
			structType = ((type == ReadType.ZERO) ? default(FSoftObjectPath) : new FSoftObjectPath(Ar));
			break;
		case "SoftObjectPath":
			structType = ((type == ReadType.ZERO) ? default(FSoftObjectPath) : new FSoftObjectPath(Ar));
			break;
		case "Timespan":
			structType = ((type == ReadType.ZERO) ? default(FDateTime) : Ar.Read<FDateTime>());
			break;
		case "UniqueNetIdRepl":
			structType = new FUniqueNetIdRepl(Ar);
			break;
		case "Vector":
			structType = ((type == ReadType.ZERO) ? default(FVector) : new FVector(Ar));
			break;
		case "Vector2D":
			structType = ((type == ReadType.ZERO) ? default(FVector2D) : new FVector2D(Ar));
			break;
		case "Vector3f":
			structType = ((type == ReadType.ZERO) ? default(TIntVector3<float>) : Ar.Read<TIntVector3<float>>());
			break;
		case "Vector4":
			structType = ((type == ReadType.ZERO) ? default(FVector4) : new FVector4(Ar));
			break;
		case "Vector4f":
			structType = ((type == ReadType.ZERO) ? default(TIntVector4<float>) : Ar.Read<TIntVector4<float>>());
			break;
		case "Vector_NetQuantize":
			structType = ((type == ReadType.ZERO) ? default(FVector) : new FVector(Ar));
			break;
		case "Vector_NetQuantize10":
			structType = ((type == ReadType.ZERO) ? default(FVector) : new FVector(Ar));
			break;
		case "Vector_NetQuantize100":
			structType = ((type == ReadType.ZERO) ? default(FVector) : new FVector(Ar));
			break;
		case "Vector_NetQuantizeNormal":
			structType = ((type == ReadType.ZERO) ? default(FVector) : new FVector(Ar));
			break;
		case "ClothLODDataCommon":
			structType = ((type == ReadType.ZERO) ? new FClothLODDataCommon() : new FClothLODDataCommon(Ar));
			break;
		case "ClothTetherData":
			structType = ((type == ReadType.ZERO) ? new FClothTetherData() : new FClothTetherData(Ar));
			break;
		case "Matrix":
			structType = ((type == ReadType.ZERO) ? new FMatrix() : new FMatrix(Ar));
			break;
		case "InstancedStruct":
			structType = new FInstancedStruct(Ar);
			break;
		case "WorldConditionQueryDefinition":
			structType = new FWorldConditionQueryDefinition(Ar);
			break;
		case "ConnectivityCube":
			structType = new FConnectivityCube(Ar);
			break;
		case "DistanceQuantity":
			structType = Ar.Read<FDistanceQuantity>();
			break;
		case "SpeedQuantity":
			structType = Ar.Read<FSpeedQuantity>();
			break;
		case "MassQuantity":
			structType = Ar.Read<FMassQuantity>();
			break;
		case "ScalarParameterValue":
			if (Ar.Game == EGame.GAME_GTATheTrilogyDefinitiveEdition)
			{
				structType = new FScalarParameterValue(Ar);
				break;
			}
			goto default;
		case "VectorParameterValue":
			if (Ar.Game == EGame.GAME_GTATheTrilogyDefinitiveEdition)
			{
				structType = new FVectorParameterValue(Ar);
				break;
			}
			goto default;
		case "TextureParameterValue":
			if (Ar.Game == EGame.GAME_GTATheTrilogyDefinitiveEdition)
			{
				structType = new FTextureParameterValue(Ar);
				break;
			}
			goto default;
		case "MaterialTextureInfo":
			if (Ar.Game == EGame.GAME_GTATheTrilogyDefinitiveEdition)
			{
				structType = new FMaterialTextureInfo(Ar);
				break;
			}
			goto default;
		case "SwBitfield_TargetRotatorMask":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_NavPermissionDetailFlags":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_NavPermissionFlags":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_NavState":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_HeroLoadoutFlags":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_HeroBufferFlags":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_HeroInputFlags":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_HeroUpgradeFlags":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_RsIkBoneTypes":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_UINavigationInput":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_WorldMapLevelType":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_WorldMapLODLevel":
			structType = new FRsBitfield(Ar, structName);
			break;
		case "RsBitfield_WorldMapWidgetFilterType":
			structType = new FRsBitfield(Ar, structName);
			break;
		default:
			structType = ((type == ReadType.ZERO) ? new FStructFallback() : ((struc != null) ? new FStructFallback(Ar, struc) : new FStructFallback(Ar, structName)));
			break;
		}
		StructType = structType;
	}

	public override string ToString()
	{
		return $"{StructType} ({StructType.GetType().Name})";
	}
}
