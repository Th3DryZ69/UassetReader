using System;
using System.Collections.Generic;
using CUE4Parse.UE4.Objects.Core.i18N;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Localization;

public class FTextLocalizationResourceConverter : JsonConverter<FTextLocalizationResource>
{
	public override void WriteJson(JsonWriter writer, FTextLocalizationResource value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		foreach (KeyValuePair<FTextKey, Dictionary<FTextKey, FEntry>> entry in value.Entries)
		{
			writer.WritePropertyName(entry.Key.Str);
			writer.WriteStartObject();
			foreach (KeyValuePair<FTextKey, FEntry> item in entry.Value)
			{
				writer.WritePropertyName(item.Key.Str);
				writer.WriteValue(item.Value.LocalizedString);
			}
			writer.WriteEndObject();
		}
		writer.WriteEndObject();
	}

	public override FTextLocalizationResource ReadJson(JsonReader reader, Type objectType, FTextLocalizationResource existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
