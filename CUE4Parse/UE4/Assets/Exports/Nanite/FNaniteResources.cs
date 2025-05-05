using CUE4Parse.UE4.Assets.Objects;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using CUE4Parse.UE4.Readers;
using CUE4Parse.UE4.Versions;

namespace CUE4Parse.UE4.Assets.Exports.Nanite;

public class FNaniteResources
{
	public FNaniteStreamableData RootData;

	public FByteBulkData StreamablePages;

	public ushort[] ImposterAtlas;

	public FPackedHierarchyNode[] HierarchyNodes;

	public uint[] HierarchyRootOffsets;

	public FPageStreamingState[] PageStreamingStates;

	public uint[] PageDependencies;

	public int NumRootPages;

	public int PositionPrecision;

	public uint NumInputTriangles;

	public uint NumInputVertices;

	public ushort NumInputMeshes;

	public ushort NumInputTexCoords;

	public uint NumClusters;

	public uint ResourceFlags;

	public FNaniteResources(FAssetArchive Ar)
	{
		if (!new FStripDataFlags(Ar).IsDataStrippedForServer())
		{
			ResourceFlags = Ar.Read<uint>();
			StreamablePages = new FByteBulkData(Ar);
			FByteArchive fByteArchive = new FByteArchive("PackedCluster", Ar.ReadArray<byte>(), Ar.Versions);
			PageStreamingStates = Ar.ReadArray<FPageStreamingState>();
			HierarchyNodes = Ar.ReadArray(() => new FPackedHierarchyNode(Ar));
			HierarchyRootOffsets = Ar.ReadArray<uint>();
			PageDependencies = Ar.ReadArray<uint>();
			ImposterAtlas = Ar.ReadArray<ushort>();
			NumRootPages = Ar.Read<int>();
			PositionPrecision = Ar.Read<int>();
			NumInputTriangles = Ar.Read<uint>();
			NumInputVertices = Ar.Read<uint>();
			NumInputMeshes = Ar.Read<ushort>();
			NumInputTexCoords = Ar.Read<ushort>();
			if (Ar.Game >= EGame.GAME_UE5_1)
			{
				NumClusters = Ar.Read<uint>();
			}
			if (PageStreamingStates.Length != 0)
			{
				fByteArchive.Position = PageStreamingStates[0].BulkOffset;
				RootData = new FNaniteStreamableData(fByteArchive, NumRootPages, PageStreamingStates[0].PageSize);
			}
		}
	}
}
