using System;
using Newtonsoft.Json;

namespace CUE4Parse.UE4.Kismet;

public class FKismetPropertyPointerConverter : JsonConverter<FKismetPropertyPointer>
{
	public override FKismetPropertyPointer? ReadJson(JsonReader reader, Type objectType, FKismetPropertyPointer? existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override void WriteJson(JsonWriter writer, FKismetPropertyPointer value, JsonSerializer serializer)
	{
		if (value.bNew)
		{
			value.New.WriteJson(writer, serializer);
		}
		else
		{
			value.Old.WriteJson(writer, serializer);
		}
	}
}
