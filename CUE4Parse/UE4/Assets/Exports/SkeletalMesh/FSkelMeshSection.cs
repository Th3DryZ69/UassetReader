using System;
using System.Collections.Generic;
using System.Linq;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Core.Math;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;
using Serilog;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

[JsonConverter(typeof(FSkelMeshSectionConverter))]
public class FSkelMeshSection
{
	public short MaterialIndex;

	public int BaseIndex;

	public int NumTriangles;

	public bool bRecomputeTangent;

	public ESkinVertexColorChannel RecomputeTangentsVertexMaskChannel;

	public bool bCastShadow;

	public bool bVisibleInRayTracing;

	[Obsolete]
	public bool bLegacyClothingSection;

	[Obsolete]
	public short CorrespondClothSectionIndex;

	public uint BaseVertexIndex;

	public FSoftVertex[] SoftVertices;

	public FMeshToMeshVertData[][] ClothMappingDataLODs;

	public ushort[] BoneMap;

	public int NumVertices;

	public int MaxBoneInfluences;

	public bool bUse16BitBoneIndex;

	public short CorrespondClothAssetIndex;

	public FClothingSectionData ClothingData;

	public Dictionary<int, int[]> OverlappingVertices;

	public bool bDisabled;

	public int GenerateUpToLodIndex;

	public int OriginalDataSectionIndex;

	public int ChunkedParentSectionIndex;

	public bool HasClothData => ClothMappingDataLODs.Any((FMeshToMeshVertData[] data) => data.Length != 0);

	public FSkelMeshSection()
	{
		RecomputeTangentsVertexMaskChannel = ESkinVertexColorChannel.Alpha;
		bCastShadow = true;
		bVisibleInRayTracing = true;
		CorrespondClothSectionIndex = -1;
		SoftVertices = Array.Empty<FSoftVertex>();
		ClothMappingDataLODs = Array.Empty<FMeshToMeshVertData[]>();
		MaxBoneInfluences = 4;
		GenerateUpToLodIndex = -1;
		OriginalDataSectionIndex = -1;
		ChunkedParentSectionIndex = -1;
	}

	public FSkelMeshSection(FAssetArchive Ar)
		: this()
	{
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		FSkeletalMeshCustomVersion.Type type = FSkeletalMeshCustomVersion.Get(Ar);
		MaterialIndex = Ar.Read<short>();
		if (type < FSkeletalMeshCustomVersion.Type.CombineSectionWithChunk)
		{
			Ar.Read<ushort>();
		}
		if (!fStripDataFlags.IsDataStrippedForServer())
		{
			BaseIndex = Ar.Read<int>();
			NumTriangles = Ar.Read<int>();
		}
		if (type < FSkeletalMeshCustomVersion.Type.RemoveTriangleSorting)
		{
			Ar.Read<byte>();
		}
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.APEX_CLOTH)
		{
			if (type < FSkeletalMeshCustomVersion.Type.DeprecateSectionDisabledFlag)
			{
				bLegacyClothingSection = Ar.ReadBoolean();
			}
			if (type < FSkeletalMeshCustomVersion.Type.RemoveDuplicatedClothingSections)
			{
				CorrespondClothSectionIndex = Ar.Read<short>();
			}
		}
		if (Ar.Ver >= EUnrealEngineObjectUE4Version.APEX_CLOTH_LOD && type < FSkeletalMeshCustomVersion.Type.RemoveEnableClothLOD)
		{
			Ar.Read<byte>();
		}
		if (FRecomputeTangentCustomVersion.Get(Ar) >= FRecomputeTangentCustomVersion.Type.RuntimeRecomputeTangent)
		{
			bRecomputeTangent = Ar.ReadBoolean();
		}
		RecomputeTangentsVertexMaskChannel = ((FRecomputeTangentCustomVersion.Get(Ar) >= FRecomputeTangentCustomVersion.Type.RecomputeTangentVertexColorMask) ? Ar.Read<ESkinVertexColorChannel>() : ESkinVertexColorChannel.Alpha);
		bCastShadow = FEditorObjectVersion.Get(Ar) < FEditorObjectVersion.Type.RefactorMeshEditorMaterials || Ar.ReadBoolean();
		bVisibleInRayTracing = FUE5MainStreamObjectVersion.Get(Ar) < FUE5MainStreamObjectVersion.Type.SkelMeshSectionVisibleInRayTracingFlagAdded || Ar.ReadBoolean();
		if (Ar.Game == EGame.GAME_TrainSimWorld2020)
		{
			Ar.Position += 8L;
		}
		if (type < FSkeletalMeshCustomVersion.Type.CombineSectionWithChunk)
		{
			return;
		}
		if (!fStripDataFlags.IsDataStrippedForServer())
		{
			BaseVertexIndex = Ar.Read<uint>();
		}
		if (!fStripDataFlags.IsEditorDataStripped() && !Ar.IsFilterEditorOnly)
		{
			if (type < FSkeletalMeshCustomVersion.Type.CombineSoftAndRigidVerts)
			{
				Ar.ReadArray(() => new FRigidVertex(Ar));
			}
			SoftVertices = Ar.ReadArray(() => new FSoftVertex(Ar));
		}
		if (FAnimObjectVersion.Get(Ar) >= FAnimObjectVersion.Type.IncreaseBoneIndexLimitPerChunk)
		{
			bUse16BitBoneIndex = Ar.ReadBoolean();
		}
		BoneMap = Ar.ReadArray<ushort>();
		if (type >= FSkeletalMeshCustomVersion.Type.SaveNumVertices)
		{
			NumVertices = Ar.Read<int>();
		}
		if (type < FSkeletalMeshCustomVersion.Type.CombineSoftAndRigidVerts)
		{
			int num = Ar.Read<int>();
			int num2 = Ar.Read<int>();
			if (num + num2 != SoftVertices.Length)
			{
				Log.Error("Legacy NumSoftVerts + NumRigidVerts != SoftVertices.Num()");
			}
		}
		MaxBoneInfluences = Ar.Read<int>();
		ClothMappingDataLODs = ((FUE5ReleaseStreamObjectVersion.Get(Ar) >= FUE5ReleaseStreamObjectVersion.Type.AddClothMappingLODBias) ? Ar.ReadArray(() => Ar.ReadArray(() => new FMeshToMeshVertData(Ar))) : new FMeshToMeshVertData[1][] { Ar.ReadArray(() => new FMeshToMeshVertData(Ar)) });
		if (type < FSkeletalMeshCustomVersion.Type.RemoveDuplicatedClothingSections)
		{
			Ar.ReadArray(() => new FVector(Ar));
			Ar.ReadArray(() => new FVector(Ar));
		}
		CorrespondClothAssetIndex = Ar.Read<short>();
		if (type < FSkeletalMeshCustomVersion.Type.NewClothingSystemAdded)
		{
			Ar.Read<short>();
		}
		else
		{
			ClothingData = Ar.Read<FClothingSectionData>();
		}
		EGame game = Ar.Game;
		if (game == EGame.GAME_KingdomHearts3 || game == EGame.GAME_FinalFantasy7Remake)
		{
			int num3 = Ar.Read<int>();
			int num4 = Ar.Read<int>();
			if (num3 == 1)
			{
				Ar.Position += ((Ar.Game == EGame.GAME_KingdomHearts3) ? (num4 * 24) : (num4 * 16));
			}
		}
		if (FOverlappingVerticesCustomVersion.Get(Ar) >= FOverlappingVerticesCustomVersion.Type.DetectOVerlappingVertices)
		{
			int num5 = Ar.Read<int>();
			OverlappingVertices = new Dictionary<int, int[]>(num5);
			for (int num6 = 0; num6 < num5; num6++)
			{
				OverlappingVertices[Ar.Read<int>()] = Ar.ReadArray<int>();
			}
		}
		if (FReleaseObjectVersion.Get(Ar) >= FReleaseObjectVersion.Type.AddSkeletalMeshSectionDisable)
		{
			bDisabled = Ar.ReadBoolean();
		}
		if (FSkeletalMeshCustomVersion.Get(Ar) >= FSkeletalMeshCustomVersion.Type.SectionIgnoreByReduceAdded)
		{
			GenerateUpToLodIndex = Ar.Read<int>();
		}
		else
		{
			GenerateUpToLodIndex = -1;
		}
		if (FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.SkeletalMeshBuildRefactor)
		{
			OriginalDataSectionIndex = Ar.Read<int>();
			ChunkedParentSectionIndex = Ar.Read<int>();
		}
		else
		{
			OriginalDataSectionIndex = -1;
			ChunkedParentSectionIndex = -1;
		}
	}

	public void SerializeRenderItem(FAssetArchive Ar)
	{
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		MaterialIndex = Ar.Read<short>();
		BaseIndex = Ar.Read<int>();
		NumTriangles = Ar.Read<int>();
		if (Ar.Game == EGame.GAME_Paragon)
		{
			Ar.Position++;
		}
		bRecomputeTangent = Ar.ReadBoolean();
		RecomputeTangentsVertexMaskChannel = ((FRecomputeTangentCustomVersion.Get(Ar) >= FRecomputeTangentCustomVersion.Type.RecomputeTangentVertexColorMask) ? Ar.Read<ESkinVertexColorChannel>() : ESkinVertexColorChannel.Alpha);
		bCastShadow = FEditorObjectVersion.Get(Ar) < FEditorObjectVersion.Type.RefactorMeshEditorMaterials || Ar.ReadBoolean();
		bVisibleInRayTracing = FUE5MainStreamObjectVersion.Get(Ar) < FUE5MainStreamObjectVersion.Type.SkelMeshSectionVisibleInRayTracingFlagAdded || Ar.ReadBoolean();
		BaseVertexIndex = Ar.Read<uint>();
		ClothMappingDataLODs = ((FUE5ReleaseStreamObjectVersion.Get(Ar) >= FUE5ReleaseStreamObjectVersion.Type.AddClothMappingLODBias) ? Ar.ReadArray(() => Ar.ReadArray(() => new FMeshToMeshVertData(Ar))) : new FMeshToMeshVertData[1][] { Ar.ReadArray(() => new FMeshToMeshVertData(Ar)) });
		BoneMap = Ar.ReadArray<ushort>();
		NumVertices = Ar.Read<int>();
		MaxBoneInfluences = Ar.Read<int>();
		CorrespondClothAssetIndex = Ar.Read<short>();
		ClothingData = Ar.Read<FClothingSectionData>();
		if (Ar.Game != EGame.GAME_Paragon)
		{
			if (Ar.Game < EGame.GAME_UE4_23 || !fStripDataFlags.IsClassDataStripped(1))
			{
				Ar.SkipFixedArray(4);
				Ar.SkipFixedArray(8);
			}
			if (FReleaseObjectVersion.Get(Ar) >= FReleaseObjectVersion.Type.AddSkeletalMeshSectionDisable)
			{
				bDisabled = Ar.ReadBoolean();
			}
			if (Ar.Game == EGame.GAME_OutlastTrials)
			{
				Ar.Position++;
			}
			EGame game = Ar.Game;
			if (game == EGame.GAME_RogueCompany || game == EGame.GAME_BladeAndSoul)
			{
				Ar.Position += 4L;
			}
		}
	}
}
