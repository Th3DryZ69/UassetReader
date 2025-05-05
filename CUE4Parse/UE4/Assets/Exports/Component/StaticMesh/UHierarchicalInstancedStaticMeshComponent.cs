using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class UHierarchicalInstancedStaticMeshComponent : UInstancedStaticMeshComponent
{
	public FClusterNode_DEPRECATED[]? ClusterTree;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		FClusterNode_DEPRECATED[] clusterTree;
		if (FReleaseObjectVersion.Get(Ar) >= FReleaseObjectVersion.Type.HISMCClusterTreeMigration)
		{
			FClusterNode_DEPRECATED[] array = Ar.ReadBulkArray(() => new FClusterNode(Ar));
			clusterTree = array;
		}
		else
		{
			clusterTree = Ar.ReadBulkArray(() => new FClusterNode_DEPRECATED(Ar));
		}
		ClusterTree = clusterTree;
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		FClusterNode_DEPRECATED[] clusterTree = ClusterTree;
		if (clusterTree != null && clusterTree.Length > 0)
		{
			writer.WritePropertyName("ClusterTree");
			serializer.Serialize(writer, ClusterTree);
		}
	}
}
