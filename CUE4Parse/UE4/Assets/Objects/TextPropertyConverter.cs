using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class TextPropertyConverter : JsonConverter<TextProperty>
{
	public override void WriteJson(JsonWriter writer, TextProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override TextProperty ReadJson(JsonReader reader, Type objectType, TextProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
