using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Wwise.Objects;

public class HierarchyConverter : JsonConverter<Hierarchy>
{
	public override void WriteJson(JsonWriter writer, Hierarchy value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Type");
		writer.WriteValue(value.Type.ToString());
		writer.WritePropertyName("Length");
		writer.WriteValue(value.Length);
		writer.WritePropertyName("Data");
		serializer.Serialize(writer, value.Data);
		writer.WriteEndObject();
	}

	public override Hierarchy ReadJson(JsonReader reader, Type objectType, Hierarchy existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
