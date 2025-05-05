using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Core.Misc;

public class FGuidConverter : JsonConverter<FGuid>
{
	public override void WriteJson(JsonWriter writer, FGuid value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString(EGuidFormats.UniqueObjectGuid));
	}

	public override FGuid ReadJson(JsonReader reader, Type objectType, FGuid existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		return new FGuid(((reader.Value as string) ?? throw new JsonSerializationException()).Replace("-", ""));
	}
}
