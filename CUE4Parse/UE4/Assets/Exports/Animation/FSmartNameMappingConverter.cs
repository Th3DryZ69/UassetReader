using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation;

public class FSmartNameMappingConverter : JsonConverter<FSmartNameMapping>
{
	public override void WriteJson(JsonWriter writer, FSmartNameMapping value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("GuidMap");
		serializer.Serialize(writer, value.GuidMap);
		writer.WritePropertyName("UidMap");
		serializer.Serialize(writer, value.UidMap);
		writer.WritePropertyName("CurveMetaDataMap");
		serializer.Serialize(writer, value.CurveMetaDataMap);
		writer.WriteEndObject();
	}

	public override FSmartNameMapping ReadJson(JsonReader reader, Type objectType, FSmartNameMapping existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
