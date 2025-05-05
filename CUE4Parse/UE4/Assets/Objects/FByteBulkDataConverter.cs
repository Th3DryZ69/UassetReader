using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class FByteBulkDataConverter : JsonConverter<FByteBulkData>
{
	public override void WriteJson(JsonWriter writer, FByteBulkData value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Header);
	}

	public override FByteBulkData ReadJson(JsonReader reader, Type objectType, FByteBulkData existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
