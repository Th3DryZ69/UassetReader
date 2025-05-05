using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.AssetRegistry;

public class FAssetRegistryStateConverter : JsonConverter<FAssetRegistryState>
{
	public override void WriteJson(JsonWriter writer, FAssetRegistryState value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("PreallocatedAssetDataBuffers");
		serializer.Serialize(writer, value.PreallocatedAssetDataBuffers);
		writer.WritePropertyName("PreallocatedDependsNodeDataBuffers");
		serializer.Serialize(writer, value.PreallocatedDependsNodeDataBuffers);
		writer.WritePropertyName("PreallocatedPackageDataBuffers");
		serializer.Serialize(writer, value.PreallocatedPackageDataBuffers);
		writer.WriteEndObject();
	}

	public override FAssetRegistryState ReadJson(JsonReader reader, Type objectType, FAssetRegistryState existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
