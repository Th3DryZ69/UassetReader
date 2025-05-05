using System;
using CUE4Parse.UE4.Objects.Core.Math;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FReferencePoseConverter : JsonConverter<FReferencePose>
{
	public override void WriteJson(JsonWriter writer, FReferencePose value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("PoseName");
		serializer.Serialize(writer, value.PoseName);
		writer.WritePropertyName("ReferencePose");
		writer.WriteStartArray();
		FTransform[] referencePose = value.ReferencePose;
		foreach (FTransform value2 in referencePose)
		{
			serializer.Serialize(writer, value2);
		}
		writer.WriteEndArray();
		writer.WriteEndObject();
	}

	public override FReferencePose ReadJson(JsonReader reader, Type objectType, FReferencePose existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
