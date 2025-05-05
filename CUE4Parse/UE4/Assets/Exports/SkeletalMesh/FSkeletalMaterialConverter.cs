using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.SkeletalMesh;

public class FSkeletalMaterialConverter : JsonConverter<FSkeletalMaterial>
{
	public override void WriteJson(JsonWriter writer, FSkeletalMaterial value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("MaterialSlotName");
		serializer.Serialize(writer, value.MaterialSlotName);
		writer.WritePropertyName("Material");
		serializer.Serialize(writer, value.Material);
		writer.WritePropertyName("ImportedMaterialSlotName");
		serializer.Serialize(writer, value.ImportedMaterialSlotName);
		writer.WritePropertyName("UVChannelData");
		serializer.Serialize(writer, value.UVChannelData);
		writer.WriteEndObject();
	}

	public override FSkeletalMaterial ReadJson(JsonReader reader, Type objectType, FSkeletalMaterial existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
