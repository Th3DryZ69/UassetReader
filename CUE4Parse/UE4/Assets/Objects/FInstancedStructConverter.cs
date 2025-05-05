using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class FInstancedStructConverter : JsonConverter<FInstancedStruct>
{
	public override void WriteJson(JsonWriter writer, FInstancedStruct value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.NonConstStruct);
	}

	public override FInstancedStruct ReadJson(JsonReader reader, Type objectType, FInstancedStruct existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
