using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class FieldPathPropertyConverter : JsonConverter<FieldPathProperty>
{
	public override void WriteJson(JsonWriter writer, FieldPathProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override FieldPathProperty ReadJson(JsonReader reader, Type objectType, FieldPathProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
