using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.Engine;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class UMorphTarget : UObject
{
	public FMorphTargetLODModel[] MorphLODModels = new FMorphTargetLODModel[1]
	{
		new FMorphTargetLODModel()
	};

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		if (!Ar.Versions["MorphTarget"])
		{
			Ar.Position = validPos;
		}
		else if (!Ar.Read<FStripDataFlags>().IsDataStrippedForServer())
		{
			MorphLODModels = Ar.ReadArray(() => new FMorphTargetLODModel(Ar));
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("MorphLODModels");
		serializer.Serialize(writer, MorphLODModels);
	}
}
