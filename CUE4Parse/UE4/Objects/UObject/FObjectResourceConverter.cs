using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FObjectResourceConverter : JsonConverter<FObjectResource>
{
	public override void WriteJson(JsonWriter writer, FObjectResource value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		if (!(value is FObjectImport fObjectImport))
		{
			if (value is FObjectExport fObjectExport)
			{
				writer.WritePropertyName("ObjectName");
				writer.WriteValue(fObjectExport.ObjectName.Text + ":" + fObjectExport.ClassName);
			}
		}
		else
		{
			writer.WritePropertyName("ObjectName");
			writer.WriteValue(fObjectImport.ObjectName.Text + ":" + fObjectImport.ClassName.Text);
		}
		writer.WritePropertyName("OuterIndex");
		serializer.Serialize(writer, value.OuterIndex);
		writer.WriteEndObject();
	}

	public override FObjectResource ReadJson(JsonReader reader, Type objectType, FObjectResource existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
