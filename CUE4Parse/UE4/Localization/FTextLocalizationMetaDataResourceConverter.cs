using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Localization;

public class FTextLocalizationMetaDataResourceConverter : JsonConverter<FTextLocalizationMetaDataResource>
{
	public override void WriteJson(JsonWriter writer, FTextLocalizationMetaDataResource value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("NativeCulture");
		writer.WriteValue(value.NativeCulture);
		writer.WritePropertyName("NativeLocRes");
		writer.WriteValue(value.NativeLocRes);
		writer.WritePropertyName("CompiledCultures");
		serializer.Serialize(writer, value.CompiledCultures);
		writer.WriteEndObject();
	}

	public override FTextLocalizationMetaDataResource ReadJson(JsonReader reader, Type objectType, FTextLocalizationMetaDataResource existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
