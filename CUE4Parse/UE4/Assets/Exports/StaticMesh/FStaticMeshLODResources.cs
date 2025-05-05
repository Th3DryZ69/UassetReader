using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Objects.Meshes;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.StaticMesh;

[JsonConverter(typeof(FStaticMeshLODResourcesConverter))]
public class FStaticMeshLODResources
{
	public enum EClassDataStripFlag : byte
	{
		CDSF_AdjacencyData = 1,
		CDSF_MinLodData = 2,
		CDSF_ReversedIndexBuffer = 4,
		CDSF_RayTracingResources = 8,
		CDSF_StripIndexBuffers = 224
	}

	public FStaticMeshSection[] Sections { get; }

	public FCardRepresentationData? CardRepresentationData { get; set; }

	public float MaxDeviation { get; }

	public FPositionVertexBuffer? PositionVertexBuffer { get; private set; }

	public FStaticMeshVertexBuffer? VertexBuffer { get; private set; }

	public FColorVertexBuffer? ColorVertexBuffer { get; private set; }

	public FRawStaticIndexBuffer? IndexBuffer { get; private set; }

	public FRawStaticIndexBuffer? ReversedIndexBuffer { get; private set; }

	public FRawStaticIndexBuffer? DepthOnlyIndexBuffer { get; private set; }

	public FRawStaticIndexBuffer? ReversedDepthOnlyIndexBuffer { get; private set; }

	public FRawStaticIndexBuffer? WireframeIndexBuffer { get; private set; }

	public FRawStaticIndexBuffer? AdjacencyIndexBuffer { get; private set; }

	public bool SkipLod
	{
		get
		{
			if (VertexBuffer != null && IndexBuffer != null && PositionVertexBuffer != null)
			{
				return ColorVertexBuffer == null;
			}
			return true;
		}
	}

	public FStaticMeshLODResources(FAssetArchive Ar)
	{
		FStripDataFlags stripDataFlags = Ar.Read<FStripDataFlags>();
		if (Ar.Game == EGame.GAME_TheDivisionResurgence)
		{
			Ar.Position += 4L;
		}
		Sections = Ar.ReadArray(() => new FStaticMeshSection(Ar));
		MaxDeviation = Ar.Read<float>();
		if (!Ar.Versions["StaticMesh.UseNewCookedFormat"])
		{
			if (!stripDataFlags.IsDataStrippedForServer() && !stripDataFlags.IsClassDataStripped(2))
			{
				SerializeBuffersLegacy(Ar, stripDataFlags);
			}
			return;
		}
		bool flag = false;
		if (Ar.Game != EGame.GAME_Splitgate)
		{
			flag = Ar.ReadBoolean();
		}
		bool flag2 = Ar.ReadBoolean() || Ar.Game == EGame.GAME_RogueCompany;
		if (stripDataFlags.IsDataStrippedForServer() || flag)
		{
			return;
		}
		if (flag2)
		{
			SerializeBuffers(Ar);
			switch (Ar.Game)
			{
			case EGame.GAME_RogueCompany:
				Ar.Position += 10L;
				break;
			case EGame.GAME_TheDivisionResurgence:
				Ar.Position += 12L;
				break;
			}
		}
		else
		{
			FByteBulkData fByteBulkData = new FByteBulkData(Ar);
			if (fByteBulkData.Header.ElementCount > 0)
			{
				FByteArchive fByteArchive = new FByteArchive("StaticMeshBufferReader", fByteBulkData.Data, Ar.Versions);
				SerializeBuffers(fByteArchive);
				fByteArchive.Dispose();
			}
			Ar.Position += 8L;
			Ar.Position += 72L;
			if (FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.RemovingTessellation)
			{
				Ar.Position += 8L;
			}
			if (Ar.Game == EGame.GAME_StarWarsJediSurvivor)
			{
				Ar.Position += 4L;
			}
		}
		Ar.Position += 12L;
		if (Ar.Game == EGame.GAME_StarWarsJediSurvivor)
		{
			Ar.Position += 4L;
		}
	}

	public void SerializeBuffersLegacy(FAssetArchive Ar, FStripDataFlags stripDataFlags)
	{
		PositionVertexBuffer = new FPositionVertexBuffer(Ar);
		VertexBuffer = new FStaticMeshVertexBuffer(Ar);
		if (Ar.Game == EGame.GAME_Borderlands3)
		{
			int num = Ar.Read<int>();
			if (num != 0)
			{
				ColorVertexBuffer = new FColorVertexBuffer(Ar);
				for (int i = 0; i < num - 1; i++)
				{
					new FColorVertexBuffer(Ar);
				}
			}
		}
		else
		{
			ColorVertexBuffer = new FColorVertexBuffer(Ar);
		}
		IndexBuffer = new FRawStaticIndexBuffer(Ar);
		if (Ar.Game != EGame.GAME_PlayerUnknownsBattlegrounds || !stripDataFlags.IsClassDataStripped(224))
		{
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.SOUND_CONCURRENCY_PACKAGE && !stripDataFlags.IsClassDataStripped(4))
			{
				ReversedIndexBuffer = new FRawStaticIndexBuffer(Ar);
				DepthOnlyIndexBuffer = new FRawStaticIndexBuffer(Ar);
				ReversedDepthOnlyIndexBuffer = new FRawStaticIndexBuffer(Ar);
			}
			else
			{
				DepthOnlyIndexBuffer = new FRawStaticIndexBuffer(Ar);
			}
			if (Ar.Ver >= EUnrealEngineObjectUE4Version.FTEXT_HISTORY && Ar.Ver < EUnrealEngineObjectUE4Version.RENAME_CROUCHMOVESCHARACTERDOWN)
			{
				new FDistanceFieldVolumeData(Ar);
			}
			if (!stripDataFlags.IsEditorDataStripped())
			{
				WireframeIndexBuffer = new FRawStaticIndexBuffer(Ar);
			}
			if (!stripDataFlags.IsClassDataStripped(1))
			{
				AdjacencyIndexBuffer = new FRawStaticIndexBuffer(Ar);
			}
		}
		if (Ar.Game > EGame.GAME_UE4_16)
		{
			for (int j = 0; j < Sections.Length; j++)
			{
				new FWeightedRandomSampler(Ar);
			}
			new FWeightedRandomSampler(Ar);
		}
		if (Ar.Game == EGame.GAME_SeaOfThieves)
		{
			Ar.Position += 17L;
		}
	}

	public void SerializeBuffers(FArchive Ar)
	{
		FStripDataFlags fStripDataFlags = Ar.Read<FStripDataFlags>();
		PositionVertexBuffer = new FPositionVertexBuffer(Ar);
		VertexBuffer = new FStaticMeshVertexBuffer(Ar);
		ColorVertexBuffer = new FColorVertexBuffer(Ar);
		if (Ar.Game == EGame.GAME_RogueCompany)
		{
			new FColorVertexBuffer(Ar);
		}
		IndexBuffer = new FRawStaticIndexBuffer(Ar);
		if (!fStripDataFlags.IsClassDataStripped(4))
		{
			ReversedIndexBuffer = new FRawStaticIndexBuffer(Ar);
		}
		DepthOnlyIndexBuffer = new FRawStaticIndexBuffer(Ar);
		if (!fStripDataFlags.IsClassDataStripped(4))
		{
			ReversedDepthOnlyIndexBuffer = new FRawStaticIndexBuffer(Ar);
		}
		if (!fStripDataFlags.IsEditorDataStripped())
		{
			WireframeIndexBuffer = new FRawStaticIndexBuffer(Ar);
		}
		if (FUE5ReleaseStreamObjectVersion.Get(Ar) < FUE5ReleaseStreamObjectVersion.Type.RemovingTessellation && !fStripDataFlags.IsClassDataStripped(1))
		{
			AdjacencyIndexBuffer = new FRawStaticIndexBuffer(Ar);
		}
		if (Ar.Versions["StaticMesh.HasRayTracingGeometry"] && !fStripDataFlags.IsClassDataStripped(8))
		{
			Ar.ReadBulkArray<byte>();
		}
		FWeightedRandomSampler[] array = new FWeightedRandomSampler[Sections.Length];
		for (int i = 0; i < Sections.Length; i++)
		{
			array[i] = new FWeightedRandomSampler(Ar);
		}
		new FWeightedRandomSampler(Ar);
	}
}
