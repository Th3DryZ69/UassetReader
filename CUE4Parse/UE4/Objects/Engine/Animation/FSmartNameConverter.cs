using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine.Animation;

public class FSmartNameConverter : JsonConverter<FSmartName>
{
	public override void WriteJson(JsonWriter writer, FSmartName value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.DisplayName);
	}

	public override FSmartName ReadJson(JsonReader reader, Type objectType, FSmartName existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
