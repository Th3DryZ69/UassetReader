using System;
using CUE4Parse.Utils;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class FByteBulkDataHeaderConverter : JsonConverter<FByteBulkDataHeader>
{
	public override void WriteJson(JsonWriter writer, FByteBulkDataHeader value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		writer.WritePropertyName("BulkDataFlags");
		writer.WriteValue(value.BulkDataFlags.ToStringBitfield());
		writer.WritePropertyName("ElementCount");
		writer.WriteValue(value.ElementCount);
		writer.WritePropertyName("SizeOnDisk");
		writer.WriteValue(value.SizeOnDisk);
		writer.WritePropertyName("OffsetInFile");
		writer.WriteValue($"0x{value.OffsetInFile:X}");
		writer.WriteEndObject();
	}

	public override FByteBulkDataHeader ReadJson(JsonReader reader, Type objectType, FByteBulkDataHeader existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
