using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class MapPropertyConverter : JsonConverter<MapProperty>
{
	public override void WriteJson(JsonWriter writer, MapProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override MapProperty ReadJson(JsonReader reader, Type objectType, MapProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
