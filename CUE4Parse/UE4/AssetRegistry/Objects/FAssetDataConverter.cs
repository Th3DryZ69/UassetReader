using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.AssetRegistry.Objects;

public class FAssetDataConverter : JsonConverter<FAssetData>
{
	public override void WriteJson(JsonWriter writer, FAssetData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("ObjectPath");
		serializer.Serialize(writer, value.ObjectPath);
		writer.WritePropertyName("PackageName");
		serializer.Serialize(writer, value.PackageName);
		writer.WritePropertyName("PackagePath");
		serializer.Serialize(writer, value.PackagePath);
		writer.WritePropertyName("AssetName");
		serializer.Serialize(writer, value.AssetName);
		writer.WritePropertyName("AssetClass");
		serializer.Serialize(writer, value.AssetClass);
		if (value.TagsAndValues.Count > 0)
		{
			writer.WritePropertyName("TagsAndValues");
			serializer.Serialize(writer, value.TagsAndValues);
		}
		if (value.TaggedAssetBundles.Bundles.Length != 0)
		{
			writer.WritePropertyName("TaggedAssetBundles");
			serializer.Serialize(writer, value.TaggedAssetBundles);
		}
		if (value.ChunkIDs.Length != 0)
		{
			writer.WritePropertyName("ChunkIDs");
			serializer.Serialize(writer, value.ChunkIDs);
		}
		if (value.PackageFlags != 0)
		{
			writer.WritePropertyName("PackageFlags");
			serializer.Serialize(writer, value.PackageFlags);
		}
		writer.WriteEndObject();
	}

	public override FAssetData ReadJson(JsonReader reader, Type objectType, FAssetData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
