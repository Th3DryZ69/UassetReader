using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class EnumPropertyConverter : JsonConverter<EnumProperty>
{
	public override void WriteJson(JsonWriter writer, EnumProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override EnumProperty ReadJson(JsonReader reader, Type objectType, EnumProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
