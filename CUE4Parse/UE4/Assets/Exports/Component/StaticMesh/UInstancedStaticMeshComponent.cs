using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Versions;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class UInstancedStaticMeshComponent : UStaticMeshComponent
{
	public FInstancedStaticMeshInstanceData[]? PerInstanceSMData;

	public float[]? PerInstanceSMCustomData;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		bool flag = false;
		if (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.SerializeInstancedStaticMeshRenderData || FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.SerializeInstancedStaticMeshRenderData)
		{
			flag = Ar.ReadBoolean();
		}
		PerInstanceSMData = Ar.ReadBulkArray(() => new FInstancedStaticMeshInstanceData(Ar));
		if (FRenderingObjectVersion.Get(Ar) >= FRenderingObjectVersion.Type.PerInstanceCustomData)
		{
			PerInstanceSMCustomData = Ar.ReadBulkArray(Ar.Read<float>);
		}
		if (flag && (FFortniteMainBranchObjectVersion.Get(Ar) >= FFortniteMainBranchObjectVersion.Type.SerializeInstancedStaticMeshRenderData || FEditorObjectVersion.Get(Ar) >= FEditorObjectVersion.Type.SerializeInstancedStaticMeshRenderData) && Ar.Read<long>() > 0)
		{
			Ar.Position = validPos;
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		FInstancedStaticMeshInstanceData[] perInstanceSMData = PerInstanceSMData;
		if (perInstanceSMData != null && perInstanceSMData.Length > 0)
		{
			writer.WritePropertyName("PerInstanceSMData");
			serializer.Serialize(writer, PerInstanceSMData);
		}
		float[] perInstanceSMCustomData = PerInstanceSMCustomData;
		if (perInstanceSMCustomData != null && perInstanceSMCustomData.Length > 0)
		{
			writer.WritePropertyName("PerInstanceSMCustomData");
			serializer.Serialize(writer, PerInstanceSMCustomData);
		}
	}
}
