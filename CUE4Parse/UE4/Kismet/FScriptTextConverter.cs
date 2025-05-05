using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class FScriptTextConverter : JsonConverter<FScriptText>
{
	public override void WriteJson(JsonWriter writer, FScriptText value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		switch (value.TextLiteralType)
		{
		case EBlueprintTextLiteralType.Empty:
			writer.WritePropertyName("SourceString");
			writer.WriteValue("");
			break;
		case EBlueprintTextLiteralType.LocalizedText:
			writer.WritePropertyName("SourceString");
			serializer.Serialize(writer, value.SourceString);
			writer.WritePropertyName("KeyString");
			serializer.Serialize(writer, value.KeyString);
			writer.WritePropertyName("Namespace");
			serializer.Serialize(writer, value.Namespace);
			break;
		case EBlueprintTextLiteralType.InvariantText:
		case EBlueprintTextLiteralType.LiteralString:
			writer.WritePropertyName("SourceString");
			serializer.Serialize(writer, value.SourceString);
			break;
		case EBlueprintTextLiteralType.StringTableEntry:
			writer.WritePropertyName("StringTableAsset");
			serializer.Serialize(writer, value.StringTableAsset);
			writer.WritePropertyName("TableIdString");
			serializer.Serialize(writer, value.TableIdString);
			writer.WritePropertyName("KeyString");
			serializer.Serialize(writer, value.KeyString);
			break;
		}
		writer.WriteEndObject();
	}

	public override FScriptText? ReadJson(JsonReader reader, Type objectType, FScriptText? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
