using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation.ACL;

public class FACLDatabaseCompressedAnimDataConverter : JsonConverter<FACLDatabaseCompressedAnimData>
{
	public override void WriteJson(JsonWriter writer, FACLDatabaseCompressedAnimData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("CompressedNumberOfFrames");
		writer.WriteValue(value.CompressedNumberOfFrames);
		writer.WritePropertyName("SequenceNameHash");
		writer.WriteValue(value.SequenceNameHash);
		writer.WriteEndObject();
	}

	public override FACLDatabaseCompressedAnimData ReadJson(JsonReader reader, Type objectType, FACLDatabaseCompressedAnimData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
