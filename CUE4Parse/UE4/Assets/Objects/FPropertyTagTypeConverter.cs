using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class FPropertyTagTypeConverter : JsonConverter<FPropertyTagType>
{
	public override void WriteJson(JsonWriter writer, FPropertyTagType value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value);
	}

	public override FPropertyTagType ReadJson(JsonReader reader, Type objectType, FPropertyTagType existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
