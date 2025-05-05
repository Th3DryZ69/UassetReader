using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class KismetExpressionConverter : JsonConverter<KismetExpression>
{
	public override void WriteJson(JsonWriter writer, KismetExpression value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		value.WriteJson(writer, serializer);
		writer.WriteEndObject();
	}

	public override KismetExpression ReadJson(JsonReader reader, Type objectType, KismetExpression existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
