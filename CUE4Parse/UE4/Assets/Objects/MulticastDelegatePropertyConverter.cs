using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Assets.Objects;

public class MulticastDelegatePropertyConverter : JsonConverter<MulticastDelegateProperty>
{
	public override void WriteJson(JsonWriter writer, MulticastDelegateProperty value, JsonSerializer serializer)
	{
		serializer.Serialize(writer, value.Value);
	}

	public override MulticastDelegateProperty ReadJson(JsonReader reader, Type objectType, MulticastDelegateProperty existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
