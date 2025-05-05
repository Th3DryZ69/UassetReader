using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Wwise.Objects;

public class AkEntryConverter : JsonConverter<AkEntry>
{
	public override void WriteJson(JsonWriter writer, AkEntry value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("NameHash");
		writer.WriteValue(value.NameHash);
		writer.WritePropertyName("OffsetMultiplier");
		writer.WriteValue(value.OffsetMultiplier);
		writer.WritePropertyName("Size");
		writer.WriteValue(value.Size);
		writer.WritePropertyName("Offset");
		writer.WriteValue(value.Offset);
		writer.WritePropertyName("FolderId");
		writer.WriteValue(value.FolderId);
		writer.WritePropertyName("Path");
		writer.WriteValue(value.Path);
		writer.WritePropertyName("IsSoundBank");
		writer.WriteValue(value.IsSoundBank);
		writer.WriteEndObject();
	}

	public override AkEntry ReadJson(JsonReader reader, Type objectType, AkEntry existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
