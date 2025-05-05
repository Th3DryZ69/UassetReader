using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FTopLevelAssetPathConverter : JsonConverter<FTopLevelAssetPath>
{
	public override void WriteJson(JsonWriter writer, FTopLevelAssetPath value, JsonSerializer serializer)
	{
		writer.WriteValue(value.ToString());
	}

	public override FTopLevelAssetPath ReadJson(JsonReader reader, Type objectType, FTopLevelAssetPath existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
