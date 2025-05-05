using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Core.Serialization;

public class FCustomVersionContainerConverter : JsonConverter<FCustomVersionContainer>
{
	public override void WriteJson(JsonWriter writer, FCustomVersionContainer? value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value?.Versions);
	}

	public override FCustomVersionContainer ReadJson(JsonReader reader, Type objectType, FCustomVersionContainer? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
