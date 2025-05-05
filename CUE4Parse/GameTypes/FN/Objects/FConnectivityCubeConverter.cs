using System;
using System.Collections;
using Newtonsoft.Json;

namespace CUE4Parse.GameTypes.FN.Objects;

public class FConnectivityCubeConverter : JsonConverter<FConnectivityCube>
{
	public override void WriteJson(JsonWriter writer, FConnectivityCube value, JsonSerializer serializer)
	{
		writer.WriteStartObject();
		for (int i = 0; i < value.Faces.Length; i++)
		{
			BitArray bitArray = value.Faces[i];
			EFortConnectivityCubeFace eFortConnectivityCubeFace = (EFortConnectivityCubeFace)i;
			writer.WritePropertyName(eFortConnectivityCubeFace.ToString());
			writer.WriteStartArray();
			for (int j = 0; j < bitArray.Length; j++)
			{
				writer.WriteValue(bitArray[j]);
			}
			writer.WriteEndArray();
		}
		writer.WriteEndObject();
	}

	public override FConnectivityCube ReadJson(JsonReader reader, Type objectType, FConnectivityCube existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
