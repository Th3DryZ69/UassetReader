using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class NamePropertyConverter : JsonConverter<NameProperty>
{
	public override void WriteJson(JsonWriter writer, NameProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override NameProperty ReadJson(JsonReader reader, Type objectType, NameProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
