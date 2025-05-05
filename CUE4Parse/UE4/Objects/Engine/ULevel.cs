using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Objects.UObject;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine;

public class ULevel : CUE4Parse.UE4.Assets.Exports.UObject
{
	public FPackageIndex[] Actors { get; private set; }

	public FURL URL { get; private set; }

	public FPackageIndex Model { get; private set; }

	public FPackageIndex[] ModelComponents { get; private set; }

	public FPackageIndex LevelScriptActor { get; private set; }

	public FPackageIndex NavListStart { get; private set; }

	public FPackageIndex NavListEnd { get; private set; }

	public FPrecomputedVisibilityHandler PrecomputedVisibilityHandler { get; private set; }

	public FPrecomputedVolumeDistanceField PrecomputedVolumeDistanceField { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		Actors = Ar.ReadArray(() => new FPackageIndex(Ar));
		URL = new FURL(Ar);
		Model = new FPackageIndex(Ar);
		ModelComponents = Ar.ReadArray(() => new FPackageIndex(Ar));
		LevelScriptActor = new FPackageIndex(Ar);
		NavListStart = new FPackageIndex(Ar);
		NavListEnd = new FPackageIndex(Ar);
		PrecomputedVisibilityHandler = new FPrecomputedVisibilityHandler(Ar);
		PrecomputedVolumeDistanceField = new FPrecomputedVolumeDistanceField(Ar);
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("Actors");
		serializer.Serialize(writer, Actors);
		writer.WritePropertyName("URL");
		serializer.Serialize(writer, URL);
		writer.WritePropertyName("Model");
		serializer.Serialize(writer, Model);
		writer.WritePropertyName("ModelComponents");
		serializer.Serialize(writer, ModelComponents);
		writer.WritePropertyName("LevelScriptActor");
		serializer.Serialize(writer, LevelScriptActor);
		writer.WritePropertyName("NavListStart");
		serializer.Serialize(writer, NavListStart);
		writer.WritePropertyName("NavListEnd");
		serializer.Serialize(writer, NavListEnd);
		writer.WritePropertyName("PrecomputedVisibilityHandler");
		serializer.Serialize(writer, PrecomputedVisibilityHandler);
		writer.WritePropertyName("PrecomputedVolumeDistanceField");
		serializer.Serialize(writer, PrecomputedVolumeDistanceField);
	}
}
