using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Objects.UObject;

public class FPackageIndexConverter : JsonConverter<FPackageIndex>
{
	public override void WriteJson(JsonWriter writer, FPackageIndex value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.ResolvedObject);
	}

	public override FPackageIndex ReadJson(JsonReader reader, Type objectType, FPackageIndex existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
