using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class LazyObjectPropertyConverter : JsonConverter<LazyObjectProperty>
{
	public override void WriteJson(JsonWriter writer, LazyObjectProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override LazyObjectProperty ReadJson(JsonReader reader, Type objectType, LazyObjectProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
