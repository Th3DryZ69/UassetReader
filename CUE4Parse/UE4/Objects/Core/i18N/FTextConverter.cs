using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.Core.i18N;

public class FTextConverter : JsonConverter<FText>
{
	public override void WriteJson(JsonWriter writer, FText value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.TextHistory);
	}

	public override FText ReadJson(JsonReader reader, Type objectType, FText existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
