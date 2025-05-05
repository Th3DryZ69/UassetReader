using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class AssetObjectPropertyConverter : JsonConverter<AssetObjectProperty>
{
	public override void WriteJson(JsonWriter writer, AssetObjectProperty value, JsonSerializer serializer)
	{
		writer.WriteValue(value.Value);
	}

	public override AssetObjectProperty ReadJson(JsonReader reader, Type objectType, AssetObjectProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
