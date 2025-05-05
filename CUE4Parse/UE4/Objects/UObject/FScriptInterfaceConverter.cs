using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FScriptInterfaceConverter : JsonConverter<FScriptInterface>
{
	public override void WriteJson(JsonWriter writer, FScriptInterface value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Object);
	}

	public override FScriptInterface ReadJson(JsonReader reader, Type objectType, FScriptInterface existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
