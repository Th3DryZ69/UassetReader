using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Engine.GameFramework;

public class FUniqueNetIdReplConverter : JsonConverter<FUniqueNetIdRepl>
{
	public override void WriteJson(JsonWriter writer, FUniqueNetIdRepl value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, (value.UniqueNetId != null) ? ((object)value.UniqueNetId) : ((object)"INVALID"));
	}

	public override FUniqueNetIdRepl ReadJson(JsonReader reader, Type objectType, FUniqueNetIdRepl existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
