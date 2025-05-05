using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Exports.Animation.ACL;

public class FACLCompressedAnimDataConverter : JsonConverter<FACLCompressedAnimData>
{
	public override void WriteJson(JsonWriter writer, FACLCompressedAnimData value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("CompressedNumberOfFrames");
		writer.WriteValue(value.CompressedNumberOfFrames);
		writer.WriteEndObject();
	}

	public override FACLCompressedAnimData ReadJson(JsonReader reader, Type objectType, FACLCompressedAnimData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
