using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class SoftObjectPropertyConverter : JsonConverter<SoftObjectProperty>
{
	public override void WriteJson(JsonWriter writer, SoftObjectProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override SoftObjectProperty ReadJson(JsonReader reader, Type objectType, SoftObjectProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
