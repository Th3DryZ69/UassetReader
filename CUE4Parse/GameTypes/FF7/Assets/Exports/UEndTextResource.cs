using System.Collections.Generic;
using CUE4Parse.GameTypes.FF7.Objects;
using CUE4Parse.UE4.Assets.Exports;
using CUE4Parse.UE4.Assets.Readers;
using CUE4Parse.UE4.Exceptions;
using Newtonsoft.Json;

namespace CUE4Parse.GameTypes.FF7.Assets.Exports;

public class UEndTextResource : UObject
{
	public Dictionary<string, FEndTextResourceStrings>? Strings;

	public override void Deserialize(FAssetArchive Ar, long validPos)
	{
		base.Deserialize(Ar, validPos);
		int num = Ar.Read<int>();
		Strings = new Dictionary<string, FEndTextResourceStrings>();
		for (int i = 0; i < num; i++)
		{
			string text = Ar.ReadFString();
			if (string.IsNullOrWhiteSpace(text) || text[0] != '$')
			{
				throw new ParserException(Ar, "EndTextResource '" + Ar.Name + "' does not start with a magic symbol!");
			}
			FEndTextResourceStrings value = new FEndTextResourceStrings(Ar);
			Strings.Add(text, value);
		}
	}

	protected internal override void WriteJson(JsonWriter writer, JsonSerializer serializer)
	{
		base.WriteJson(writer, serializer);
		Dictionary<string, FEndTextResourceStrings>? strings = Strings;
		if (strings == null || strings.Count <= 0)
		{
			return;
		}
		writer.WritePropertyName("Strings");
		writer.WriteStartObject();
		foreach (var (name, fEndTextResourceStrings2) in Strings)
		{
			if (fEndTextResourceStrings2.Entries != null)
			{
				Dictionary<string, string>? entries = fEndTextResourceStrings2.Entries;
				if (entries == null || entries.Count > 0)
				{
					writer.WritePropertyName(name);
					serializer.Serialize(writer, fEndTextResourceStrings2.Entries);
				}
			}
		}
		writer.WriteEndObject();
	}
}
