using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets;

public class ResolvedObjectConverter : JsonConverter<ResolvedObject>
{
	public override void WriteJson(JsonWriter writer, ResolvedObject value, JsonSerializer serializer)
	{
		ResolvedObject resolvedObject = value;
		while (true)
		{
			ResolvedObject outer = resolvedObject.Outer;
			if (outer == null)
			{
				break;
			}
			resolvedObject = outer;
		}
		ResolvedObject resolvedObject2 = resolvedObject;
		writer.WriteStartObject();
		writer.WritePropertyName("ObjectName");
		writer.WriteValue(value.GetFullName(includeOuterMostName: false));
		writer.WritePropertyName("ObjectPath");
		string text = resolvedObject2.Name.Text;
		writer.WriteValue((value.ExportIndex != -1) ? $"{text}.{value.ExportIndex}" : text);
		writer.WriteEndObject();
	}

	public override ResolvedObject ReadJson(JsonReader reader, Type objectType, ResolvedObject existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
