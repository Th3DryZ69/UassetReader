using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class SetPropertyConverter : JsonConverter<SetProperty>
{
	public override void WriteJson(JsonWriter writer, SetProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override SetProperty ReadJson(JsonReader reader, Type objectType, SetProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
