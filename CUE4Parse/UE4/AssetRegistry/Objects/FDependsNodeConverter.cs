using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FDependsNodeConverter : JsonConverter<FDependsNode>
{
	public override void WriteJson(JsonWriter writer, FDependsNode value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("Identifier");
		serializer.Serialize(writer, value.Identifier);
		WriteDependsNodeList("PackageDependencies", writer, value.PackageDependencies);
		WriteDependsNodeList("NameDependencies", writer, value.NameDependencies);
		WriteDependsNodeList("ManageDependencies", writer, value.ManageDependencies);
		WriteDependsNodeList("Referencers", writer, value.Referencers);
		if (value.PackageFlags != null)
		{
			writer.WritePropertyName("PackageFlags");
			serializer.Serialize(writer, value.PackageFlags);
		}
		if (value.ManageFlags != null)
		{
			writer.WritePropertyName("ManageFlags");
			serializer.Serialize(writer, value.ManageFlags);
		}
		writer.WriteEndObject();
	}

	private static void WriteDependsNodeList(string name, JsonWriter writer, List<FDependsNode> dependsNodeList)
	{
		if (dependsNodeList.Count == 0)
		{
			return;
		}
		writer.WritePropertyName(name);
		writer.WriteStartArray();
		foreach (FDependsNode dependsNode in dependsNodeList)
		{
			writer.WriteValue(dependsNode._index);
		}
		writer.WriteEndArray();
	}

	public override FDependsNode ReadJson(JsonReader reader, Type objectType, FDependsNode existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
