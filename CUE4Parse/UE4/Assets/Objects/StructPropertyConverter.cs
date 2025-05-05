using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class StructPropertyConverter : JsonConverter<StructProperty>
{
	public override void WriteJson(JsonWriter writer, StructProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override StructProperty ReadJson(JsonReader reader, Type objectType, StructProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
