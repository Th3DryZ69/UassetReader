using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class UScriptStructConverter : JsonConverter<UScriptStruct>
{
	public override void WriteJson(JsonWriter writer, UScriptStruct value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.StructType);
	}

	public override UScriptStruct ReadJson(JsonReader reader, Type objectType, UScriptStruct existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
