using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Assets.Objects;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FFormatContainerConverter : JsonConverter<FFormatContainer>
{
	public override void WriteJson(JsonWriter writer, FFormatContainer value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		foreach (KeyValuePair<FName, FByteBulkData> format in value.Formats)
		{
			writer.WritePropertyName(format.Key.Text);
			serializer.Serialize(writer, format.Value);
		}
		writer.WriteEndObject();
	}

	public override FFormatContainer ReadJson(JsonReader reader, Type objectType, FFormatContainer existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
