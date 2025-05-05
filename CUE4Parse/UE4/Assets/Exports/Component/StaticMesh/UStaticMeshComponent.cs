using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Component.StaticMesh;

public class UStaticMeshComponent : UObject
{
	public FStaticMeshComponentLODInfo[]? LODData;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		LODData = Ar.ReadArray(() => new FStaticMeshComponentLODInfo(Ar));
	}

	public FPackageIndex GetStaticMesh()
	{
		FPackageIndex fPackageIndex = new FPackageIndex();
		UStaticMeshComponent uStaticMeshComponent = this;
		while (true)
		{
			fPackageIndex = uStaticMeshComponent.GetOrDefault("StaticMesh", new FPackageIndex());
			if (!fPackageIndex.IsNull || uStaticMeshComponent.Template == null)
			{
				break;
			}
			uStaticMeshComponent = uStaticMeshComponent.Template.Load<UStaticMeshComponent>();
		}
		return fPackageIndex;
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		FStaticMeshComponentLODInfo[] lODData = LODData;
		if (lODData == null || lODData.Length > 0)
		{
			writer.WritePropertyName("LODData");
			serializer.Serialize(writer, LODData);
		}
	}
}
