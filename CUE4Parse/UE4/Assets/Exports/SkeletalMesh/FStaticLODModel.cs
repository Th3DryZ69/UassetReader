using System;
using CUE4Parse.UE4.Assets.Exports.StaticMesh;
using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

[JsonConverter(typeof(FStaticLODModelConverter))]
public class FStaticLODModel
{
	public FSkelMeshSection[] Sections;

	public FMultisizeIndexContainer? Indices;

	public short[] ActiveBoneIndices;

	public FSkelMeshChunk[] Chunks;

	public int Size;

	public int NumVertices;

	public short[] RequiredBones;

	public FIntBulkData RawPointIndices;

	public int[] MeshToImportVertexMap;

	public int MaxImportVertex;

	public int NumTexCoords;

	public FMorphTargetVertexInfoBuffers? MorphTargetVertexInfoBuffers;

	public FSkeletalMeshVertexBuffer VertexBufferGPUSkin;

	public FSkeletalMeshVertexColorBuffer ColorVertexBuffer;

	public FMultisizeIndexContainer AdjacencyIndexBuffer;

	public FSkeletalMeshVertexClothBuffer ClothVertexBuffer;

	public bool SkipLod
	{
		get
		{
			if (Indices != null)
			{
				if (Indices.Indices16.Length < 1)
				{
					return Indices.Indices32.Length < 1;
				}
				return false;
			}
			return true;
		}
	}

	public FStaticLODModel()
	{
		Chunks = Array.Empty<FSkelMeshChunk>();
		MeshToImportVertexMap = Array.Empty<int>();
		ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer();
	}

	public FStaticLODModel(FAssetArchive Ar, bool bHasVertexColors)
		: this()
	{
		if (Ar.Game == EGame.GAME_SeaOfThieves)
		{
			Ar.Position += 4L;
		}
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		FSkeletalMeshCustomVersion.Type type = FSkeletalMeshCustomVersion.Get(Ar);
		if (Ar.Game == EGame.GAME_SeaOfThieves)
		{
			Ar.Position += 4L;
		}
		Sections = Ar.ReadArray(() => new FSkelMeshSection(Ar));
		if (type < FSkeletalMeshCustomVersion.Type.SplitModelAndRenderData)
		{
			Indices = new FMultisizeIndexContainer(Ar);
		}
		else
		{
			Indices = new FMultisizeIndexContainer
			{
				Indices32 = Ar.ReadBulkArray<uint>()
			};
		}
		ActiveBoneIndices = Ar.ReadArray<short>();
		if (type < FSkeletalMeshCustomVersion.Type.CombineSectionWithChunk)
		{
			Chunks = Ar.ReadArray(() => new FSkelMeshChunk(Ar));
		}
		Size = Ar.Read<int>();
		if (!fStripDataFlags.IsDataStrippedForServer())
		{
			NumVertices = Ar.Read<int>();
		}
		RequiredBones = Ar.ReadArray<short>();
		if (!fStripDataFlags.IsEditorDataStripped())
		{
			RawPointIndices = new FIntBulkData(Ar);
		}
		if (Ar.Game != EGame.GAME_StateOfDecay2 && Ar.Ver >= EUnrealEngineObjectUE4Version.ADD_SKELMESH_MESHTOIMPORTVERTEXMAP)
		{
			MeshToImportVertexMap = Ar.ReadArray<int>();
			MaxImportVertex = Ar.Read<int>();
		}
		if (!fStripDataFlags.IsDataStrippedForServer())
		{
			NumTexCoords = Ar.Read<int>();
			if (type < FSkeletalMeshCustomVersion.Type.SplitModelAndRenderData)
			{
				VertexBufferGPUSkin = new FSkeletalMeshVertexBuffer(Ar);
				if (type >= FSkeletalMeshCustomVersion.Type.UseSeparateSkinWeightBuffer)
				{
					FSkinWeightVertexBuffer fSkinWeightVertexBuffer = new FSkinWeightVertexBuffer(Ar, VertexBufferGPUSkin.bExtraBoneInfluences);
					if (fSkinWeightVertexBuffer.Weights.Length != 0)
					{
						if (VertexBufferGPUSkin.bUseFullPrecisionUVs)
						{
							for (int num = 0; num < NumVertices; num++)
							{
								VertexBufferGPUSkin.VertsFloat[num].Infs = fSkinWeightVertexBuffer.Weights[num];
							}
						}
						else
						{
							for (int num2 = 0; num2 < NumVertices; num2++)
							{
								VertexBufferGPUSkin.VertsHalf[num2].Infs = fSkinWeightVertexBuffer.Weights[num2];
							}
						}
					}
				}
				if (bHasVertexColors)
				{
					if (type < FSkeletalMeshCustomVersion.Type.UseSharedColorBufferFormat)
					{
						ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(Ar);
					}
					else
					{
						FColorVertexBuffer fColorVertexBuffer = new FColorVertexBuffer(Ar);
						ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(fColorVertexBuffer.Data);
					}
				}
				if (Ar.Ver < EUnrealEngineObjectUE4Version.REMOVE_EXTRA_SKELMESH_VERTEX_INFLUENCES)
				{
					throw new ParserException("Unsupported: extra SkelMesh vertex influences (old mesh format)");
				}
				if (Ar.Game == EGame.GAME_StateOfDecay2)
				{
					Ar.Position += 8L;
					return;
				}
				if (Ar.Game == EGame.GAME_SeaOfThieves)
				{
					int num3 = Ar.Read<int>();
					Ar.Position += num3 * 44;
					for (int num4 = 0; num4 < 4; num4++)
					{
						Ar.ReadArray<int>();
					}
					Ar.Position += 13L;
				}
				if (Ar.Game == EGame.GAME_FinalFantasy7Remake)
				{
					int num5 = Ar.Read<int>();
					if (num5 >= 10)
					{
						Ar.Position -= 4L;
						AdjacencyIndexBuffer = new FMultisizeIndexContainer(Ar);
					}
					num5 = Ar.Read<int>();
					if (num5 == 0 || num5 == 1)
					{
						return;
					}
					Ar.Position -= 4L;
					if (new FStripDataFlags(Ar).IsClassDataStripped(1))
					{
						Ar.Position -= 2L;
						return;
					}
					int num6 = Ar.Read<int>();
					int num7 = Ar.Read<int>();
					if (num7 < 30)
					{
						Ar.Position -= 10L;
						return;
					}
					Ar.Position += num6 * num7;
					ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(Ar);
					return;
				}
				if (!fStripDataFlags.IsClassDataStripped(1))
				{
					AdjacencyIndexBuffer = new FMultisizeIndexContainer(Ar);
				}
				if (Ar.Ver >= EUnrealEngineObjectUE4Version.APEX_CLOTH && HasClothData())
				{
					ClothVertexBuffer = new FSkeletalMeshVertexClothBuffer(Ar);
				}
			}
		}
		if (Ar.Game == EGame.GAME_SeaOfThieves)
		{
			new FMultisizeIndexContainer(Ar);
		}
	}

	public void SerializeRenderItem(FAssetArchive Ar, bool bHasVertexColors, byte numVertexColorChannels)
	{
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		bool flag = false;
		if (Ar.Game != EGame.GAME_Splitgate)
		{
			flag = Ar.ReadBoolean();
		}
		bool flag2 = Ar.ReadBoolean();
		RequiredBones = Ar.ReadArray<short>();
		if (!fStripDataFlags.IsDataStrippedForServer() && !flag)
		{
			Sections = new FSkelMeshSection[Ar.Read<int>()];
			for (int i = 0; i < Sections.Length; i++)
			{
				Sections[i] = new FSkelMeshSection();
				Sections[i].SerializeRenderItem(Ar);
			}
			ActiveBoneIndices = Ar.ReadArray<short>();
			if (Ar.Game == EGame.GAME_KenaBridgeofSpirits)
			{
				Ar.ReadArray<byte>();
			}
			Ar.Position += 4L;
			if (flag2)
			{
				SerializeStreamedData(Ar, bHasVertexColors);
				if (Ar.Game == EGame.GAME_RogueCompany)
				{
					Ar.Position += 12L;
					int num = Ar.Read<int>();
					int num2 = Ar.Read<int>();
					if (num > 0 && num2 > 0)
					{
						Ar.SkipBulkArrayData();
					}
				}
			}
			else
			{
				FByteBulkData fByteBulkData = new FByteBulkData(Ar);
				if (fByteBulkData.Header.ElementCount > 0)
				{
					using (FByteArchive ar = new FByteArchive("LodReader", fByteBulkData.Data, Ar.Versions))
					{
						SerializeStreamedData(ar, bHasVertexColors);
					}
					int num3 = 5;
					if (FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.RemovingTessellation && !fStripDataFlags.IsClassDataStripped(1))
					{
						num3 += 5;
					}
					num3 += 32;
					num3 += FSkinWeightVertexBuffer.MetadataSize(Ar);
					Ar.Position += num3;
					if (Ar.Game == EGame.GAME_StarWarsJediSurvivor)
					{
						Ar.Position += 4L;
					}
					if (HasClothData())
					{
						long[] array = Ar.ReadArray<long>();
						Ar.Position += 8L;
						if (FUE5ReleaseStreamObjectVersion.Get(Ar) >= FUE5ReleaseStreamObjectVersion.Type.AddClothMappingLODBias)
						{
							Ar.Position += 4 * array.Length;
						}
					}
					Ar.ReadArray(Ar.ReadFName);
				}
			}
		}
		if (Ar.Game == EGame.GAME_ReadyOrNot)
		{
			Ar.Position += 4L;
		}
	}

	public void SerializeRenderItem_Legacy(FAssetArchive Ar, bool bHasVertexColors, byte numVertexColorChannels)
	{
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		Sections = new FSkelMeshSection[Ar.Read<int>()];
		for (int i = 0; i < Sections.Length; i++)
		{
			Sections[i] = new FSkelMeshSection();
			Sections[i].SerializeRenderItem(Ar);
		}
		Indices = new FMultisizeIndexContainer(Ar);
		VertexBufferGPUSkin = new FSkeletalMeshVertexBuffer
		{
			bUseFullPrecisionUVs = true
		};
		ActiveBoneIndices = Ar.ReadArray<short>();
		RequiredBones = Ar.ReadArray<short>();
		if (!fStripDataFlags.IsDataStrippedForServer() && !fStripDataFlags.IsClassDataStripped(2))
		{
			FPositionVertexBuffer fPositionVertexBuffer = new FPositionVertexBuffer(Ar);
			FStaticMeshVertexBuffer fStaticMeshVertexBuffer = new FStaticMeshVertexBuffer(Ar);
			FSkinWeightVertexBuffer fSkinWeightVertexBuffer = new FSkinWeightVertexBuffer(Ar, VertexBufferGPUSkin.bExtraBoneInfluences);
			if (!bHasVertexColors && Ar.Game == EGame.GAME_Borderlands3)
			{
				for (int j = 0; j < numVertexColorChannels; j++)
				{
					FColorVertexBuffer fColorVertexBuffer = new FColorVertexBuffer(Ar);
					ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(fColorVertexBuffer.Data);
				}
			}
			else if (bHasVertexColors)
			{
				FColorVertexBuffer fColorVertexBuffer2 = new FColorVertexBuffer(Ar);
				ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(fColorVertexBuffer2.Data);
			}
			if (!fStripDataFlags.IsClassDataStripped(1))
			{
				AdjacencyIndexBuffer = new FMultisizeIndexContainer(Ar);
			}
			if (HasClothData())
			{
				ClothVertexBuffer = new FSkeletalMeshVertexClothBuffer(Ar);
			}
			NumVertices = fPositionVertexBuffer.NumVertices;
			NumTexCoords = fStaticMeshVertexBuffer.NumTexCoords;
			VertexBufferGPUSkin.VertsFloat = new FGPUVertFloat[NumVertices];
			for (int k = 0; k < VertexBufferGPUSkin.VertsFloat.Length; k++)
			{
				VertexBufferGPUSkin.VertsFloat[k] = new FGPUVertFloat
				{
					Pos = fPositionVertexBuffer.Verts[k],
					Infs = fSkinWeightVertexBuffer.Weights[k],
					Normal = fStaticMeshVertexBuffer.UV[k].Normal,
					UV = fStaticMeshVertexBuffer.UV[k].UV
				};
			}
		}
		if (Ar.Game >= EGame.GAME_UE4_23)
		{
			new FSkinWeightProfilesData(Ar);
		}
	}

	private void SerializeStreamedData(FArchive Ar, bool bHasVertexColors)
	{
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		Indices = new FMultisizeIndexContainer(Ar);
		VertexBufferGPUSkin = new FSkeletalMeshVertexBuffer
		{
			bUseFullPrecisionUVs = true
		};
		FPositionVertexBuffer fPositionVertexBuffer = new FPositionVertexBuffer(Ar);
		FStaticMeshVertexBuffer fStaticMeshVertexBuffer = new FStaticMeshVertexBuffer(Ar);
		FSkinWeightVertexBuffer fSkinWeightVertexBuffer = new FSkinWeightVertexBuffer(Ar, VertexBufferGPUSkin.bExtraBoneInfluences);
		if (bHasVertexColors)
		{
			FColorVertexBuffer fColorVertexBuffer = new FColorVertexBuffer(Ar);
			ColorVertexBuffer = new FSkeletalMeshVertexColorBuffer(fColorVertexBuffer.Data);
		}
		if (FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.RemovingTessellation && !fStripDataFlags.IsClassDataStripped(1))
		{
			AdjacencyIndexBuffer = new FMultisizeIndexContainer(Ar);
		}
		if (HasClothData())
		{
			ClothVertexBuffer = new FSkeletalMeshVertexClothBuffer(Ar);
		}
		new FSkinWeightProfilesData(Ar);
		if (Ar.Versions["SkeletalMesh.HasRayTracingData"])
		{
			Ar.ReadArray<byte>();
		}
		if (FUE5PrivateFrostyStreamObjectVersion.Get(Ar) >= FUE5PrivateFrostyStreamObjectVersion.Type.SerializeSkeletalMeshMorphTargetRenderData && Ar.ReadBoolean())
		{
			MorphTargetVertexInfoBuffers = new FMorphTargetVertexInfoBuffers(Ar);
		}
		NumVertices = fPositionVertexBuffer.NumVertices;
		NumTexCoords = fStaticMeshVertexBuffer.NumTexCoords;
		VertexBufferGPUSkin.VertsFloat = new FGPUVertFloat[NumVertices];
		for (int i = 0; i < VertexBufferGPUSkin.VertsFloat.Length; i++)
		{
			VertexBufferGPUSkin.VertsFloat[i] = new FGPUVertFloat
			{
				Pos = fPositionVertexBuffer.Verts[i],
				Infs = fSkinWeightVertexBuffer.Weights[i],
				Normal = fStaticMeshVertexBuffer.UV[i].Normal,
				UV = fStaticMeshVertexBuffer.UV[i].UV
			};
		}
	}

	private bool HasClothData()
	{
		for (int i = 0; i < Chunks.Length; i++)
		{
			if (Chunks[i].HasClothData)
			{
				return true;
			}
		}
		for (int j = 0; j < Sections.Length; j++)
		{
			if (Sections[j].HasClothData)
			{
				return true;
			}
		}
		return false;
	}
}
