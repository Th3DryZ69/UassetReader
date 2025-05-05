using CUE4Parse.UE4.Assets.Readers;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Wwise;

public class UAkMediaAssetData : UObject
{
	public bool IsStreamed { get; private set; }

	public bool UseDeviceMemory { get; private set; }

	public FAkMediaDataChunk[] DataChunks { get; private set; }

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		IsStreamed = GetOrDefault("IsStreamed", defaultValue: false);
		UseDeviceMemory = GetOrDefault("UseDeviceMemory", defaultValue: false);
		DataChunks = Ar.ReadArray(() => new FAkMediaDataChunk(Ar));
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		writer.WritePropertyName("DataChunks");
		writer.WriteStartArray();
		FAkMediaDataChunk[] dataChunks = DataChunks;
		foreach (FAkMediaDataChunk value in dataChunks)
		{
			serializer.Serialize(writer, value);
		}
		writer.WriteEndArray();
	}
}
