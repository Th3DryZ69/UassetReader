using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Material;

public class FMaterialParameterInfoConverter : JsonConverter<FMaterialParameterInfo>
{
	public override void WriteJson(JsonWriter writer, FMaterialParameterInfo value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Name");
		serializer.Serialize(writer, value.Name);
		writer.WritePropertyName("Association");
		writer.WriteValue("EMaterialParameterAssociation::" + value.Association);
		writer.WritePropertyName("Index");
		writer.WriteValue(value.Index);
		writer.WriteEndObject();
	}

	public override FMaterialParameterInfo ReadJson(JsonReader reader, Type objectType, FMaterialParameterInfo existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
