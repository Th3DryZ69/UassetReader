using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class ArrayPropertyConverter : JsonConverter<ArrayProperty>
{
	public override void WriteJson(JsonWriter writer, ArrayProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override ArrayProperty ReadJson(JsonReader reader, Type objectType, ArrayProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
