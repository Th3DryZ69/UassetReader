using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.WorldCondition;

public class FWorldConditionQueryDefinitionConverter : JsonConverter<FWorldConditionQueryDefinition>
{
	public override void WriteJson(JsonWriter writer, FWorldConditionQueryDefinition value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.StaticStruct);
	}

	public override FWorldConditionQueryDefinition ReadJson(JsonReader reader, Type objectType, FWorldConditionQueryDefinition existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
