using CUE4Parse.UE4.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Nanite;

public class FNaniteStreamableData
{
	[JsonIgnore]
	public FFixupChunk FixupChunk;

	public FHierarchyFixup[] HierarchyFixups;

	public FClusterFixup[] ClusterFixups;

	public FRootPageInfo[] RootPageInfos;

	public FCluster[] Clusters;

	public unsafe FNaniteStreamableData(FArchive Ar, int numRootPages, uint pageSize)
	{
		FixupChunk = Ar.Read<FFixupChunk>();
		HierarchyFixups = Ar.ReadArray(FixupChunk.Header.NumHierachyFixups, () => new FHierarchyFixup(Ar));
		ClusterFixups = Ar.ReadArray(FixupChunk.Header.NumClusterFixups, () => new FClusterFixup(Ar));
		RootPageInfos = Ar.ReadArray<FRootPageInfo>(numRootPages);
		Clusters = Ar.ReadArray(0, () => new FCluster(Ar));
		Ar.Position += pageSize - sizeof(FRootPageInfo) * numRootPages;
	}
}
