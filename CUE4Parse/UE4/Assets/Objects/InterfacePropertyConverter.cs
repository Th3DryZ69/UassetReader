using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class InterfacePropertyConverter : JsonConverter<InterfaceProperty>
{
	public override void WriteJson(JsonWriter writer, InterfaceProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override InterfaceProperty ReadJson(JsonReader reader, Type objectType, InterfaceProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
