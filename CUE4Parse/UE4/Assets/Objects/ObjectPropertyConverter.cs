using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class ObjectPropertyConverter : JsonConverter<ObjectProperty>
{
	public override void WriteJson(JsonWriter writer, ObjectProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override ObjectProperty ReadJson(JsonReader reader, Type objectType, ObjectProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
