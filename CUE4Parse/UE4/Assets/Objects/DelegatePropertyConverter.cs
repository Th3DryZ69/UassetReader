using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class DelegatePropertyConverter : JsonConverter<DelegateProperty>
{
	public override void WriteJson(JsonWriter writer, DelegateProperty value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Num");
		writer.WriteValue(value.Num);
		writer.WritePropertyName("Name");
		serializer.Serialize(writer, value.Value);
		writer.WriteEndObject();
	}

	public override DelegateProperty ReadJson(JsonReader reader, Type objectType, DelegateProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
